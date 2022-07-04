namespace VueViteCore.Business.Common;

public record StorageResponse(string SASToken, string BlobUrl, long TokenExpiration);

