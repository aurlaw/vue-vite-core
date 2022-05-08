using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueViteCore.Business.Persistence;
using VueViteCore.Models;

namespace VueViteCore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _applicationDb;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDb)
    {
        _logger = logger;
        _applicationDb = applicationDb;
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
}