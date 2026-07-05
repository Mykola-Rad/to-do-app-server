using ToDoApp.Application.DTOs.Categories;
using ToDoApp.Application.DTOs.Tasks.Steps;

namespace ToDoApp.Application.DTOs.Tasks
{
    public record TaskResponseDto(
        int Id,
        string Title,
        string? Description,
        bool IsCompleted,
        DateTime CreatedAt,
        DateTime? CompletedAt,
        DateTime? DueDate,
        CategoryDto Category,
        IEnumerable<TaskStepResponseDto> Steps
    );
}
