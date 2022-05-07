using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
// using VueViteCore.Business.Identity;
using VueViteCore.Business.Persistence;

namespace VueViteCore.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("VueViteCoreDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        
        // services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        //     .AddRoles<IdentityRole>()
        //     .AddEntityFrameworkStores<ApplicationDbContext>();
        
        
        // services.AddTransient<IIdentityService, IdentityService>();


        services.AddAuthentication()
            .AddIdentityCookies();
        
        services.AddAuthorization(options => 
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));
        
        return services;
    }
    
    public static async Task  PerformMigrationsIfNecessary(this WebApplication app)
    {
        // migrate any database changes on startup (includes initial db creation)
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
        try
        {
            logger.LogInformation("Perform migration");
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (dbContext.Database.IsSqlServer())
            {
                await dbContext.Database.MigrateAsync();
            }
                
            // seed 
            logger.LogInformation("Seed data");
            await ApplicationDbContextSeed.SeedSampleDataAsync(dbContext);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the database");
            throw;
        }
    }
    
}
/*
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();

                if (context.Database.IsSqlServer())
                {
                    context.Database.Migrate();
                }

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
                await ApplicationDbContextSeed.SeedSampleDataAsync(context);
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }
        }
 *
 * 
 */