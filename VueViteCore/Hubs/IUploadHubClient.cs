namespace VueViteCore.Hubs;

public interface IUploadHubClient
{

    Task SendReceive(Uri uploadedUrl);
    Task SendProgress(string message);
    Task SendError(string error);
}
