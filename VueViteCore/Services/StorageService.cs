using Microsoft.Extensions.Options;
using VueViteCore.Business.Common;
using VueViteCore.Business.Common.Interfaces;
using VueViteCore.Models;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace VueViteCore.Services;

public class StorageService : IStorageService
{
    private readonly AzureStorageSettings _storageSettings;
    private readonly ILogger<StorageService> _logger;

    public StorageService(IOptions<AzureStorageSettings> storageSettings, ILogger<StorageService> logger)
    {
        _logger = logger;
        _storageSettings = storageSettings.Value;
    }
    
    public  Task<StorageResponse> GenerateSASToken(string container)
    {
        if(_storageSettings.ExpirationHours <= 0) 
        {
            throw new ArgumentOutOfRangeException(nameof(_storageSettings.ExpirationHours));
        }
        // create storage key
        var storageKey = new StorageSharedKeyCredential(_storageSettings.AccountName, _storageSettings.Key);
        // Create a SAS token that's valid for 24 hours.
        var sasBuilder = new AccountSasBuilder()
        {
            Services = AccountSasServices.Blobs | AccountSasServices.Files,
            ResourceTypes = AccountSasResourceTypes.All,
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(_storageSettings.ExpirationHours),
            Protocol = SasProtocol.Https
        };
        sasBuilder.SetPermissions(AccountSasPermissions.All);
        // Use the key to get the SAS token.
        var sasToken = sasBuilder.ToSasQueryParameters(storageKey);
        _logger.LogInformation("SAS token generated {SasToken}", sasToken);
        var response = new StorageResponse(
            sasToken.ToString(),
            $"{_storageSettings.BlobUrl}/{container}",
            sasToken.ExpiresOn.ToUnixTimeSeconds()
        );
        return Task.FromResult(response);        
    }

    private Task<BlobContainerClient> GetContainerAsync(string container)
    {
        var blobServiceClient = new BlobServiceClient(_storageSettings.ConnectionString);
        return Task.FromResult(blobServiceClient.GetBlobContainerClient(container));
    }

}