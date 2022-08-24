using System.Drawing;
using System.Globalization;
using DocumentFormat.OpenXml.Wordprocessing;

namespace VueViteCore;

public static class ColorExtensions
{
    public static int? Hexify(this string? hexValue)
    {
        if (hexValue is null)
        {
            return null;
        }

        try
        {
            var color = ColorTranslator.FromHtml(hexValue);

            return (color.R << 16) + (color.G << 8) + color.B;
        }
        catch
        {
            return null;
        }
    }

    public static int? ConvertToBGR(this string? hexValue)
    {
        if (hexValue is null)
        {
            return null;
        }
        try
        {
            var color = ColorTranslator.FromHtml(hexValue);

            return (255 << 24) + (color.B << 16) + (color.G << 8) + color.R;
        }
        catch
        {
            return null;
        }


    }
    
}
//unchecked((int)0xFF5e4935)
//unchecked((int)0xFF5e4935)),//ABGR  83b841  (RGB:41b883)