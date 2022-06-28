using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using VueViteCore.Business.Persistence;
using VueViteCore.Hubs;
using VueViteCore.Models;
using VueViteCore.Services.Hosted;

namespace VueViteCore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _applicationDb;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IHubContext<UploadHub, IUploadHubClient> _hubContext;
    private readonly IWebHostEnvironment _environment;

    public HomeController(
        ILogger<HomeController> logger, 
        ApplicationDbContext applicationDb, 
        IBackgroundTaskQueue taskQueue, 
        IHubContext<UploadHub, IUploadHubClient> hubContext, IWebHostEnvironment environment)
    {
        _logger = logger;
        _applicationDb = applicationDb;
        _taskQueue = taskQueue;
        _hubContext = hubContext;
        _environment = environment;
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
