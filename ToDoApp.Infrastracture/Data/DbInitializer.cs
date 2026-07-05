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
                new() { Name = "Work" },
                new() { Name = "Personal" },
                new() { Name = "Learning" },
                new() { Name = "Sports" }
            };

            await context.Categories.AddRangeAsync(globalCategories);
            await context.SaveChangesAsync();

            Console.WriteLine("Database seeding completed successfully.");
        }
    }
}
