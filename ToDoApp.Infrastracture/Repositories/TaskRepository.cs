using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;
using ToDoApp.Domain.Repositories;
using ToDoApp.Infrastracture.Data;

namespace ToDoApp.Infrastracture.Repositories;

public class TaskRepository : RepositoryBase<ToDoTask>, ITaskRepository
{
    public TaskRepository(ToDoDbContext context) : base(context)
    {
    }

    public async Task<ToDoTask?> GetWithStepsByIdAsync(int id, int userId)
    {
        return await _context.ToDoTasks
            .Include(t => t.Steps)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task<IEnumerable<ToDoTask>> GetPagedTasksAsync(
        int userId,
        int? categoryId,
        string? searchTerm,
        TaskFilterType filter,
        int pageNumber,
        int pageSize)
    {
        var query = GetFilteredTasksQuery(userId, categoryId, searchTerm, filter)
            .AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.Steps);

        return await query
            .OrderBy(t => t.IsCompleted)
            .ThenBy(t => t.DueDate == null)
            .ThenBy(t => t.DueDate)
            .ThenByDescending(t => t.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(
        int userId,
        int? categoryId,
        string? searchTerm,
        TaskFilterType filter)
    {
        var query = GetFilteredTasksQuery(userId, categoryId, searchTerm, filter);
        return await query.CountAsync();
    }

    private IQueryable<ToDoTask> GetFilteredTasksQuery(
        int userId,
        int? categoryId,
        string? searchTerm,
        TaskFilterType filter)
    {
        var query = _context.ToDoTasks.Where(t => t.UserId == userId);

        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var normalizedSearch = searchTerm.ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(normalizedSearch) ||
                                    (t.Description != null
                                        && t.Description.ToLower().Contains(normalizedSearch)));
        }

        var today = DateTime.UtcNow.Date;

        query = filter switch
        {
            TaskFilterType.Today => query.Where(t => t.DueDate != null && t.DueDate.Value.Date == today),
            TaskFilterType.Planned => query.Where(t => t.DueDate != null),
            _ => query
        };

        return query;
    }
}