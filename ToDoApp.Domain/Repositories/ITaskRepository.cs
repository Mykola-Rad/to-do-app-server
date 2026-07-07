using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Domain.Repositories
{
    public interface ITaskRepository : IRepositoryBase<ToDoTask>
    {
        Task<ToDoTask?> GetWithStepsByIdAsync(int id, int userId);
        Task<IEnumerable<ToDoTask>> GetPagedTasksAsync(
            int userId,
            int? categoryId,
            string? searchTerm,
            TaskFilterType filter, 
            int pageNumber,
            int pageSize);

        Task<int> GetTotalCountAsync(
            int userId,
            int? categoryId,
            string? searchTerm,
            TaskFilterType filter);

        Task DeleteMultipleAsync(IEnumerable<int> ids, int userId);
    }
}