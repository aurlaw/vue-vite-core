namespace VueViteCore.Business.Common.Interfaces;

public interface IStorageService
{
    /// <summary>
    /// Generates a SAS token for Azure Storage
    /// </summary>
    /// <param name="container">Name of storage container</param>
    /// <returns></returns>
    Task<StorageResponse> GenerateSASToken(string container);    
}