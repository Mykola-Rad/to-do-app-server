using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastracture.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();

            Console.WriteLine("Checking and applying migrations...");
            await context.Database.MigrateAsync();

            if (await context.Categories.AnyAsync())
            {
                Console.WriteLine("Database already seeded. Skipping...");
                return;
            }

            Console.WriteLine("Seeding global categories...");

            var globalCategories = new List<Category>
            {
                new() { Name = "Work", ColorHex = "#4A90E2" },      
                new() { Name = "Personal", ColorHex = "#2ECC71" }, 
                new() { Name = "Learning", ColorHex = "#9B59B6" }, 
                new() { Name = "Sports", ColorHex = "#E67E22" }    
            };

            await context.Categories.AddRangeAsync(globalCategories);
            await context.SaveChangesAsync();

            Console.WriteLine("Database seeding completed successfully.");
        }
    }
}
