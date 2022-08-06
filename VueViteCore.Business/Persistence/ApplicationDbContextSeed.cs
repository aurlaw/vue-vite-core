using Microsoft.AspNetCore.Identity;
using VueViteCore.Business.Entities;
using VueViteCore.Business.Identity;

namespace VueViteCore.Business.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed, if necessary
        if (!context.TodoLists.Any())
        {
            context.TodoLists.Add(new TodoList
            {
                Title = "Shopping",
                Items =
                {
                    new TodoItem { Title = "Apples" },
                    new TodoItem { Title = "Milk" },
                    new TodoItem { Title = "Bread" },
                    new TodoItem { Title = "Toilet paper" },
                    new TodoItem { Title = "Pasta" },
                    new TodoItem { Title = "Tissues" },
                    new TodoItem { Title = "Tuna" },
                    new TodoItem { Title = "Water" }
                }
            });

            await context.SaveChangesAsync();
        }


        if (!context.SubmissionEntries.Any())
        {
            context.SubmissionEntries.Add(new SubmissionEntry
            {
                Name = "John Doe",
                ValueOne = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin lectus enim, auctor molestie euismod in, pretium ut purus. In hac habitasse platea dictumst. Fusce elit quam, semper at vehicula sit amet, consequat vel magna. Nulla facilisi. Aliquam et sapien massa. Nunc pharetra aliquet aliquam. Aliquam porttitor dictum diam non porta. ",
                ValueTwo = "Sed sodales diam orci, id malesuada metus luctus auctor. Praesent vel felis eros. Proin neque risus, congue sed eros sit amet, tincidunt accumsan urna. Interdum et malesuada fames ac ante ipsum primis in faucibus. Aenean commodo arcu nibh. Suspendisse potenti. Proin ut tortor vitae lacus pretium interdum. Etiam in ex faucibus, imperdiet justo sed, facilisis diam. ",
                ValueThree = "Donec metus dui, elementum sit amet accumsan et, cursus vitae elit. Duis rhoncus viverra tellus at varius. Quisque ultrices felis ut ornare posuere. Phasellus sem libero, egestas quis justo eget, eleifend maximus quam. Phasellus in lectus sed tortor rutrum tempus et in justo. Ut congue ligula vitae augue elementum faucibus. Mauris et risus et sem fermentum venenatis. Sed ut venenatis dolor. Vestibulum gravida felis ut tortor vehicula, sed tincidunt metus placerat. Duis eget ex lectus. ",
                Created = DateTime.UtcNow
            });
            context.SubmissionEntries.Add(new SubmissionEntry
            {
                Name = "Jane Doe",
                ValueOne = "Integer convallis arcu at felis faucibus, scelerisque condimentum libero lobortis. Etiam quis nisi convallis, volutpat magna quis, dignissim ex. Donec porta, risus at scelerisque scelerisque, urna erat luctus dui, id ultrices tellus ligula non sem. In et volutpat odio, sed dignissim justo. Sed vitae posuere lectus. Phasellus feugiat risus ut placerat fermentum. Donec et vestibulum tellus. ",
                ValueTwo = "Cras iaculis odio id nisi feugiat dapibus. Suspendisse ligula mi, faucibus sed finibus ac, convallis aliquet ante. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aliquam a convallis nisi. Duis vel pretium tellus. Morbi lacinia, quam a faucibus vestibulum, dui purus bibendum orci, vitae tristique ipsum nunc quis nunc. In ac suscipit mauris. Fusce porta porttitor nulla, sed blandit lectus ultrices in. Aliquam erat volutpat. Etiam tristique diam tellus, sit amet bibendum felis porta et. Aenean viverra nisl et lorem venenatis condimentum. Etiam diam nulla, ultricies non suscipit id, ultrices at nisi. Morbi sem metus, tincidunt vel ullamcorper at, aliquam at turpis. Curabitur sit amet augue justo. ",
                ValueThree = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin lectus enim, auctor molestie euismod in, pretium ut purus. In hac habitasse platea dictumst. Fusce elit quam, semper at vehicula sit amet, consequat vel magna. Nulla facilisi. Aliquam et sapien massa. Nunc pharetra aliquet aliquam. Aliquam porttitor dictum diam non porta. ",
                Created = DateTime.UtcNow
            });
        
            await context.SaveChangesAsync();
        }
    }
    
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var administratorRole = new IdentityRole("Administrator");

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
        }

        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Administrator1!");
            await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }
    }
}
