namespace ToDoApp.Domain.Entities
{
    public class ToDoTaskStep
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int ToDoTaskId { get; set; }
        public ToDoTask ToDoTask { get; set; } = null!;
    }
}
