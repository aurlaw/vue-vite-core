namespace VueViteCore.Models;

public class AzureStorageSettings
{
    public string ConnectionString { get; set; } = null!;
    public string QueueName { get; set; } = null!;
    public string SiteContainer { get; set; } = null!;
    public string BlobUrl { get; set; } = null;
    public int ExpirationHours {get;set;}
    public string AccountName {get;set;}= null;
    public string Key {get;set;}= null;
    

}