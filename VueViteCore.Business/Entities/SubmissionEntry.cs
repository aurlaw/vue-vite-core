namespace VueViteCore.Business.Entities;

public class SubmissionEntry
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ValueOne { get; set; }
    public string? ValueTwo { get; set; }
    public string? ValueThree { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    
}