namespace ToDoApp.Application.DTOs.Tasks
{
    public record UpdateTaskDto(
        string Title,
        string? Description,
        DateTime? DueDate,
        int CategoryId
    );
}
