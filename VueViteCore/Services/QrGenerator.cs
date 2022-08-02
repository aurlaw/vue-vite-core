using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace VueViteCore.Services;

public class QrGenerator
{
    private readonly ILogger<QrGenerator> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public QrGenerator(IWebHostEnvironment webHostEnvironment, ILogger<QrGenerator> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    public Task<(byte[], string)> GenerateFromLink(string link, string? logoPath, QrAttribute? attribute)
    {
        
        var generator = new PayloadGenerator.Url(link);
        var payload = generator.ToString();
        
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        byte[]? qrCodeAsBytes;
        if (attribute is null)
        {
            qrCodeAsBytes = qrCode.GetGraphic(20);
        }
        else
        {
            qrCodeAsBytes = qrCode.GetGraphic(20);
        }
        //Bitmap qrCodeImage = qrCode.GetGraphic(20, "#000ff0", "#0ff000");

        return Task.FromResult((qrCodeAsBytes, "image/png"));

        // if (!string.IsNullOrEmpty(logoPath))
        // {
        //     var logoFile = Path.Combine(_webHostEnvironment.WebRootPath, logoPath);
        //     
        // }
    }


    public async Task<(byte[], string)>  GenerateFromLinkV2(string link, string? logoPath)
    {
        var qrCodeWriter = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = 500,
                Width = 500,
                Margin = 0
            },
            Renderer = new PixelDataRenderer {
                Foreground = new PixelDataRenderer.Color(unchecked((int)0xFF000000)),//BGR
                Background = new PixelDataRenderer.Color(unchecked((int)0xFFFFFFFF)),
            }            
        };        
        var pixelData = qrCodeWriter.Write(link);      
        byte[] byteArray;
        using (var image = Image.LoadPixelData<Rgba32>(pixelData.Pixels, 500, 500))
        {
            var logo = Path.Combine(_webHostEnvironment.WebRootPath, "assets/logo.png");
            var logoImg = await Image.LoadAsync(logo);
            // Calculate the delta height and width between QR code and logo
            var deltaHeight = image.Height - logoImg.Height;
            var deltaWidth = image.Width - logoImg.Width;
            _logger.LogInformation("{DeltaHeight} = {Image.Height} - {LogoImg.Height}", 
                deltaHeight, image.Height, logoImg.Height);
            var location = new Point
            {
                X = (int) Math.Round((double) (deltaWidth / 2)),
                Y =  (int) Math.Round((double) (deltaHeight / 2))
            };

            image.Mutate(x => x.DrawImage(logoImg, location, 1f));
            
            using var ms = new MemoryStream();
            await image.SaveAsPngAsync(ms);
            byteArray = ms.ToArray();
        }

        return (byteArray, "image/png");
    }
}

/*

 
 * var bw = new ZXing.BarcodeWriter();

        var encOptions = new ZXing.Common.EncodingOptions
        {
            Width = width,
            Height = height,
            Margin = 0,
            PureBarcode = false
        };

        encOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

        bw.Renderer = new BitmapRenderer();
        bw.Options = encOptions;
        bw.Format = ZXing.BarcodeFormat.QR_CODE;
        Bitmap bm = bw.Write(text);
        Bitmap overlay = new Bitmap(imagePath);

        int deltaHeigth = bm.Height - overlay.Height;
        int deltaWidth = bm.Width - overlay.Width;

        Graphics g = Graphics.FromImage(bm);
        g.DrawImage(overlay, new Point(deltaWidth/2,deltaHeigth/2));

        return bm;
 *
 * 
 */

public class QrAttribute
{
    public string DarkColor { get; set; } = null!;
    public string LightColor { get; set; } = null!;
}
