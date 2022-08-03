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
    private async void DoWork(object? state)
    {
        _logger.LogInformation("Timed Hosted Service is working: {QueueName}", _settings.QueueName);
        //https://briancaos.wordpress.com/2021/06/16/net-core-queuemessage-json-to-typed-object-using-azure-storage-queues/
        if (!await _queueClient.ExistsAsync()) return;

        QueueProperties properties = await _queueClient.GetPropertiesAsync();
        _logger.LogInformation("Message Count: {ApproximateMessagesCount}", properties.ApproximateMessagesCount);
        
        if (properties.ApproximateMessagesCount <= 0) return;
        
        QueueMessage[] retrievedMessage = await _queueClient.ReceiveMessagesAsync(1);    
        if(retrievedMessage.Length > 0) 
        {
            _logger.LogInformation("Messages Length: {Length}", retrievedMessage.Length);
            var result = retrievedMessage[0].As<MediaResult>();                    
            _logger.LogInformation("Message: {@Result} {Video}", result, result.Video);

        }

    }
    
    
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