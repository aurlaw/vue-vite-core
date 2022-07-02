namespace VueViteCore.Models;

public class MediaResult
{
    public Uri Thumbnail { get; set; }= null!;
    public Uri Video { get; set; }= null!;
    public MediaAttributes Attributes { get; set; }    = null!;
}