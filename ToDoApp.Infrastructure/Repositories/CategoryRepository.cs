using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Repositories;
using ToDoApp.Infrastructure.Data;

namespace ToDoApp.Infrastructure.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ToDoDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetCategoriesForUserAsync(int userId)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
    }
}
