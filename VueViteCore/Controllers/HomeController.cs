using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using VueViteCore.Business.Common.Interfaces;
using VueViteCore.Business.Entities;
using VueViteCore.Business.Persistence;
using VueViteCore.Hubs;
using VueViteCore.Models;
using VueViteCore.Models.Maps;
using VueViteCore.Services;
using VueViteCore.Services.Hosted;

namespace VueViteCore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IApplicationDbContext _applicationDb;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IHubContext<UploadHub, IUploadHubClient> _hubContext;
    private readonly IWebHostEnvironment _environment;
    private readonly IStorageService _storageService;
    private readonly AzureStorageSettings _storageSettings;
    private readonly ExportService _exportService;

    public HomeController(
        ILogger<HomeController> logger, 
        IApplicationDbContext applicationDb, 
        IBackgroundTaskQueue taskQueue, 
        IHubContext<UploadHub, IUploadHubClient> hubContext, 
        IWebHostEnvironment environment, 
        IStorageService storageService, 
        IOptions<AzureStorageSettings> storageSettings, 
        ExportService exportService)
    {
        _logger = logger;
        _applicationDb = applicationDb;
        _taskQueue = taskQueue;
        _hubContext = hubContext;
        _environment = environment;
        _storageService = storageService;
        _exportService = exportService;
        _storageSettings = storageSettings.Value;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Contact()
    {
        return View();
    }
    public IActionResult Router()
    {
        return View();
    }
    public async Task<IActionResult> Editor()
    {
        
        var model = new EditorViewModel
        {
            Page  = "editor_page"
        };
        var pageRegion = await _applicationDb.PageRegions
            .Where(p => p.Page == model.Page)
            .ToListAsync();
        foreach (var region in pageRegion)
        {
            model.Regions.Add(region.Region, region.Content);
        }
        //
        if (!Request.Cookies.ContainsKey("sastoken"))
        {
            var storage = await _storageService.GenerateSASToken(_storageSettings.SiteContainer);
            var expires = DateTimeOffset.FromUnixTimeSeconds(storage.TokenExpiration);
            Response.Cookies.Append("sastoken", JsonSerializer.Serialize(storage), new CookieOptions()
            {
                Expires = expires
            });
            
        }
        return View(model);
    }
    public IActionResult Csv()
    {
        return View();
    }
    public async Task<IActionResult> DownloadCsv(CancellationToken cancellationToken)
    {
        var entries = await GetSubmissions(cancellationToken);
        var result = await _exportService.GetExportedStream<SubmissionEntry, SubmissionEntryMap>(entries,
            ExportType.Csv, "entry_submission_", cancellationToken);

        return File(result.Stream, result.ContentType, result.FileName);


    }
    public async Task<IActionResult> DownloadXlsx(CancellationToken cancellationToken)
    {
        var entries = await GetSubmissions(cancellationToken);
        var result = await _exportService.GetExportedStream<SubmissionEntry, SubmissionEntryMap>(entries,
            ExportType.Excel, "entry_submission_", cancellationToken);

        return File(result.Stream, result.ContentType, result.FileName);

    }

    private async Task<IList<SubmissionEntry>> GetSubmissions(CancellationToken cancellationToken)
    {
        return await _applicationDb
            .SubmissionEntries
            .ToListAsync(cancellationToken);
    }



    // public IActionResult GrapeEditor()
    // {
    //     return View();
    // }

    public IActionResult QrCode()
    {
        var model = new QrViewModel();
        return View(model);
    }
    [HttpPost]
    public IActionResult QrCode(QrViewModel model)
    {
        var param = new
        {
            name = "mycode",
            model.Link,
            model.LogoPath,
            model.DarkColor
        };
        var qrCodeUrl = Url.ActionLink(nameof(CodeController.Index), "Code", param, protocol: Request.Scheme);
        
        model.QrCode = qrCodeUrl;
        return View(model);
    }

    public IActionResult Video()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    [HttpGet("/api/todos")]
    public async Task<IActionResult> GetTodos()
    {
        var todos = await _applicationDb.TodoItems.ToListAsync();
        return Ok(todos);
    }

    [HttpPost("/api/savepage")]
    public async Task<IActionResult> SavePage([FromForm]EditorViewModel? model, CancellationToken token)
    {
        if (model is null)
        {
            _logger.LogInformation("No data saved");
        }
        else
        {
            _logger.LogInformation("Page editor: {Page}", model.Page);
            foreach (var modelRegion in model.Regions)
            {
                _logger.LogInformation("Region: {@ModelRegion}", modelRegion);
            }

            await SavePageContent(model, token);
        }

        return NoContent();
    }

    private async Task SavePageContent(EditorViewModel pageRegion, CancellationToken token = default)
    {
        foreach (var data in pageRegion.Regions)
        {

            var entity = await _applicationDb.PageRegions.FirstOrDefaultAsync(p =>
                p.Page == pageRegion.Page && p.Region == data.Key, token);
            if (entity is null)
            {
                entity = new PageRegion
                {
                    Page = pageRegion.Page,
                    Region = data.Key,
                    Created = DateTime.UtcNow
                };
                _applicationDb.PageRegions.Add(entity);
            }
            else
            {
                entity.Modified = DateTime.UtcNow;
            }

            entity.Content = data.Value;

            await _applicationDb.SaveChangesAsync(token);

        }
    }
    
    
    [HttpPost("/api/upload")]
    [RequestFormLimits(MultipartBodyLengthLimit = 150000000)]
    [RequestSizeLimit(150000000)]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken token)
    {
        var groupId = "MyUserId";
        //await _hubClient.Clients.Groups(request.GroupId).SendProgress(msg);
        if (file.Length > 0)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, token);
            ms.Position = 0;

            await _hubContext.Clients.Groups(groupId).SendProgress("Uploading...");
            // add to task queue
            await _taskQueue.QueueBackgroundWorkItemAsync(async token =>
            {
                _logger.LogInformation("Queue started...");
                await SaveMedia(groupId, file.FileName, file.ContentType, ms.ToArray(), token);
                _logger.LogInformation("Queue Completed");
            });
            
            return Ok(new { Success = true,  file.Name, file.ContentType});

        }
        return Ok(new { Success = false});
    }

    private async ValueTask SaveMedia(string groupId, string fileName, string contentType, byte[] fileData, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SaveMedia:  {FileName} - {ContentType}", fileName, contentType);
        try
        {
            await _hubContext.Clients.Groups(groupId).SendProgress("Processing...");

            var output = Path.Combine(_environment.WebRootPath, "output");
            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }

            var fileUrl = string.Concat("/output/", fileName);
            var filePath = Path.Combine(output, fileName);

            using var image = Image.Load(fileData, out var format);
            const int max = 400;
            var (w, h) = GetResizedDims(image, max);
            await _hubContext.Clients.Groups(groupId).SendProgress($"Dims: {w}x{h} with max:{max}");

            image.Mutate(c => c.Resize(w,h));
            using var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, format, cancellationToken);
            await using var outputFileStream = new FileStream(filePath, FileMode.Create);
            outputStream.Position = 0;
            await outputStream.CopyToAsync(outputFileStream, cancellationToken);  
            
            await _hubContext.Clients.Groups(groupId).SendReceive(new Uri(fileUrl));
        }
        catch (Exception ex)
        {
            await _hubContext.Clients.Groups(groupId).SendError(ex.Message);
        }
    }

    private (int, int) GetResizedDims(Image image, int maxDim)
    {
        if (image.Width < maxDim && image.Height < maxDim)
        {
            return (image.Width, image.Height);
        }
        
        var ratioX = (double)maxDim / image.Width;
        var ratioY = (double)maxDim / image.Height;
        var ratio = Math.Min(ratioX, ratioY);
        
        var newWidth = (int)(image.Width * ratio);
        var newHeight = (int)(image.Height * ratio);

        return (newWidth, newHeight);
    }
    
    
}
