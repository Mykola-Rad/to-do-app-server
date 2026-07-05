using FluentResults;
using ToDoApp.Application.DTOs.Common;
using ToDoApp.Application.DTOs.Tasks;
using ToDoApp.Application.DTOs.Tasks.Steps;

namespace ToDoApp.Application.Services.Interfaces
{
    public interface ITaskService
    {
        Task<Result<TaskResponseDto>> GetByIdAsync(int id, int userId);
        Task<Result<PagedResponseDto<TaskResponseDto>>> GetPagedAsync(
            int userId, int? categoryId, string? searchTerm, bool? isToday, int pageNumber, int pageSize);
        Task<Result<TaskResponseDto>> CreateAsync(int userId, CreateTaskDto dto);
        Task<Result<TaskResponseDto>> UpdateAsync(int id, int userId, UpdateTaskDto dto);
        Task<Result> DeleteAsync(int id, int userId);
        Task<Result> ToggleTaskAsync(int id, int userId);
        Task<Result<TaskStepResponseDto>> AddStepAsync(int taskId, int userId, AddStepDto dto);
        Task<Result> RemoveStepAsync(int taskId, int stepId, int userId);
        Task<Result> ToggleStepAsync(int taskId, int stepId, int userId);
    }
}
