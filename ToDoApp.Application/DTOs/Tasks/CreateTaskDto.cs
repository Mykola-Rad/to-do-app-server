namespace ToDoApp.Application.DTOs.Tasks
{
    public record CreateTaskDto(
        string Title,
        DateTime? DueDate,
        int CategoryId
    );
}
