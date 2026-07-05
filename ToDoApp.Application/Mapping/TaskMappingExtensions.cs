using ToDoApp.Application.DTOs.Tasks;
using ToDoApp.Application.DTOs.Tasks.Steps;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Mapping
{
    public static class TaskMappingExtensions
    {
        public static TaskResponseDto ToDto(this ToDoTask task)
        {
            if (task == null) return null!;

            return new TaskResponseDto(
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.CreatedAt,
                task.CompletedAt,
                task.DueDate,
                task.Category.ToDto(),
                task.Steps?.Select(step => step.ToDto()) ?? Enumerable.Empty<TaskStepResponseDto>()
            );
        }

        public static ToDoTask ToEntity(this CreateTaskDto dto, int userId)
        {
            return new ToDoTask
            {
                Title = dto.Title,
                DueDate = dto.DueDate,
                CategoryId = dto.CategoryId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                IsCompleted = false,
                CompletedAt = null,
                Description = null 
            };
        }

        public static void UpdateEntity(this UpdateTaskDto dto, ToDoTask task)
        {
            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;
            task.CategoryId = dto.CategoryId;
        }
    }
}
