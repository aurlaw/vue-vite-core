using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using VueViteCore.Services;
using System;

namespace VueViteCore.Controllers;

public class CodeController : Controller
{
    private readonly QrGenerator _qrGenerator;
    
    public CodeController(QrGenerator qrGenerator)
    {
        _qrGenerator = qrGenerator;
    }

    // GET /code?name=&link=&logoPath
    public async Task Index([FromQuery] string name, string link, string? logoPath, string? darkColor)
    {
        var (imgBytes, contentType) = await _qrGenerator.GenerateFromLinkV2(link, logoPath, darkColor);
        Response.ContentType = contentType;
        Response.ContentLength = imgBytes.Length;
        await Response.Body.WriteAsync(imgBytes);

    }
}

