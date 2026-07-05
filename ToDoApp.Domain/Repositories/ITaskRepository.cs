using ToDoApp.Domain.Entities;

namespace ToDoApp.Domain.Repositories
{
    public interface ITaskRepository : IRepositoryBase<ToDoTask>
    {
        Task<ToDoTask?> GetWithStepsByIdAsync(int id, int userId);
        Task<IEnumerable<ToDoTask>> GetPagedTasksAsync(
            int userId,
            int? categoryId,
            string? searchTerm,
            bool? isToday,
            int pageNumber,
            int pageSize);

        Task<int> GetTotalCountAsync(
            int userId, 
            int? categoryId, 
            string? searchTerm,
            bool? isToday);

        Task<IEnumerable<ToDoTask>> GetTasksByDateRangeAsync(
            int userId, 
            DateTime startDate, 
            DateTime endDate);
    }
}