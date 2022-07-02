namespace VueViteCore.Models;

public class MediaAttributes
{
    public Uri UploadUri { get; set; } = null!;
    public string Callback { get; set; }= null!;
    public DateTimeOffset CreatedAt { get; set; }
}
