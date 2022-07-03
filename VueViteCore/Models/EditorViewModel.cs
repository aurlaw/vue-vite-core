using System.Text.Json.Serialization;

namespace VueViteCore.Models;

public class EditorViewModel
{
    [JsonPropertyName("page")]
    public string Page { get; set; } = null!;
    [JsonPropertyName("regions")]
    public Dictionary<string, string> Regions { get; set; } = new();
}