using VueViteCore.Business;
using VueViteCore.Business.Persistence;
using Serilog;
using VueViteCore.Business.Identity;
using VueViteCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VueViteCore.Hubs;
using VueViteCore.Services.Hosted;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

// DI
// builder.Services.AddSingleton<Manifest>();
    builder.Services.AddBusiness(builder.Configuration);
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>();

    builder.Services.AddSingleton<IBackgroundTaskQueue>(ctx =>
    {
        if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
            queueCapacity = 100;
        return new BackgroundTaskQueue(queueCapacity);
    });
    builder.Services.AddHostedService<QueuedHostedService>();
 
// scaffold razor identity UI
//https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=netcore-cli


// Add services to the container.
    builder.Services.AddRouting(x => x.LowercaseUrls = true);
    var mvcBuilder = builder.Services.AddControllersWithViews();

    if (builder.Environment.IsDevelopment())
    {
        // allows us to change razor views without recompile
        mvcBuilder.AddRazorRuntimeCompilation();
    }
    builder.Services.AddSignalR();

    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHealthChecks("/health");
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<UploadHub>("/uploadHub");

// EF migrations
    await app.PerformMigrationsIfNecessary();

    app.Run();    
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
