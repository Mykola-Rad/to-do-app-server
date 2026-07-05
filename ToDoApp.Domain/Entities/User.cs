namespace ToDoApp.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<ToDoTask> Tasks { get; set; } = new List<ToDoTask>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
