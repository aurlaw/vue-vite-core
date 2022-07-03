namespace VueViteCore.Business.Entities;

public class PageRegion
{
    public int Id { get; set; }
    public string Page { get; set; } = null!;
    public string Region { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }

}