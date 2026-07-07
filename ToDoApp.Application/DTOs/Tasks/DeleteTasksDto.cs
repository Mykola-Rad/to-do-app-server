namespace ToDoApp.Application.DTOs.Tasks
{
    public class DeleteTasksDto
    {
        public IEnumerable<int> Ids { get; set; } = new List<int>();
    }
}
