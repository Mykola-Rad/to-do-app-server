namespace ToDoApp.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string ColorHex { get; set; } = "#808080";
        public int? UserId { get; set; }
        public User? User { get; set; }

        public ICollection<ToDoTask> Tasks { get; set; } = new List<ToDoTask>();
    }
}
