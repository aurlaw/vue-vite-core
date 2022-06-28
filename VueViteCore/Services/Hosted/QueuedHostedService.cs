namespace VueViteCore.Services.Hosted;

public class QueuedHostedService : BackgroundService
{
    private readonly ILogger<QueuedHostedService> _logger;


    public QueuedHostedService(ILogger<QueuedHostedService> logger, IBackgroundTaskQueue taskQueue)
    {
        _logger = logger;
        TaskQueue = taskQueue;
    }
    public IBackgroundTaskQueue TaskQueue { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queued Hosted Service is running");

        await BackgroundProcessing(stoppingToken);            
    }
    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = 
                await TaskQueue.DequeueAsync(stoppingToken);

            try
            {
                await workItem(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error occurred executing {WorkItem}", nameof(workItem));
            }
        }
    }    
    
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queued Hosted Service is stopping");

        await base.StopAsync(stoppingToken);
    }

}