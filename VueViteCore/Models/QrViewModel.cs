using System.ComponentModel.DataAnnotations;

namespace VueViteCore.Models;

public class QrViewModel
{
    [Display(Name = "Logo")]
    public string LogoPath { get; set; } = null!;
    [Required]
    [Display(Name = "Link")]
    public string Link { get; set; } = null!;

    public string DarkColor { get; set; } = null!;
    public string LightColor { get; set; } = null!;
    public string QrCode { get; set; } = null!;
}
