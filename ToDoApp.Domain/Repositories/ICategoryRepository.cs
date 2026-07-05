using ToDoApp.Domain.Entities;

namespace ToDoApp.Domain.Repositories
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesForUserAsync(int userId);
    }
}
