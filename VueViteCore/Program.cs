// using VueViteCore;

using VueViteCore.Business;
using VueViteCore.Business.Persistence;

var builder = WebApplication.CreateBuilder(args);

// DI
// builder.Services.AddSingleton<Manifest>();
builder.Services.AddBusiness(builder.Configuration);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// services.AddSingleton<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

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

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

// EF migrations
await app.PerformMigrationsIfNecessary();

app.Run();