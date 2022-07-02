using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using NuGet.Protocol;
using VueViteCore.Models;

namespace VueViteCore.Services.Hosted;

public class StorageQueueHostedService : IHostedService, IDisposable
{
    private readonly ILogger<StorageQueueHostedService> _logger;
    private readonly AzureStorageSettings _settings;
    private Timer? _timer = null;
    private readonly QueueClient _queueClient;

    public StorageQueueHostedService(ILogger<StorageQueueHostedService> logger, IConfiguration config)
    {
        _logger = logger;
        _settings = config.GetSection("AzureStorage").Get<AzureStorageSettings>();
        _queueClient = new QueueClient(_settings.ConnectionString, _settings.QueueName);

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start Timer");
        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }
    private void DoWork(object? state)
    {
        _logger.LogInformation("Timed Hosted Service is working: {QueueName}", _settings.QueueName);
        Task.Run(async () =>
        {
            if (await _queueClient.ExistsAsync())
            {
                //https://briancaos.wordpress.com/2021/06/16/net-core-queuemessage-json-to-typed-object-using-azure-storage-queues/
                QueueProperties properties = await _queueClient.GetPropertiesAsync();
                _logger.LogInformation("Message Count: {ApproximateMessagesCount}", properties.ApproximateMessagesCount);
                if (properties.ApproximateMessagesCount > 0)
                {
                    QueueMessage[] retrievedMessage = await _queueClient.ReceiveMessagesAsync(1);    
                    _logger.LogInformation("Messages Length: {Length}", retrievedMessage.Length);
                    var result = retrievedMessage[0].As<MediaResult>();                    
                    _logger.LogInformation("Message: {@Result} {Video}", result, result.Video);
                }
            }
        });
    }
    
    
    /*
     *static async Task<string> RetrieveNextMessageAsync(QueueClient theQueue)
{
    if (await theQueue.ExistsAsync())
    {
        QueueProperties properties = await theQueue.GetPropertiesAsync();

        if (properties.ApproximateMessagesCount > 0)
        {
            QueueMessage[] retrievedMessage = await theQueue.ReceiveMessagesAsync(1);
            string theMessage = retrievedMessage[0].Body.ToString();
            await theQueue.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
            return theMessage;
        }

        return null;
    }

    return null;
}
     * 
     */
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stop Timer");
        _timer?.Change(Timeout.Infinite, 0);
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}