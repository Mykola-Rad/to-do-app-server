namespace ToDoApp.Domain.Entities
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<ToDoTaskStep> Steps { get; set; } = new List<ToDoTaskStep>();
    }
}
