namespace VueViteCore.Models;

public class AzureStorageSettings
{
    public string ConnectionString { get; set; } = null!;
    public string QueueName { get; set; } = null!;
    public string SiteContainer { get; set; } = null!;

}