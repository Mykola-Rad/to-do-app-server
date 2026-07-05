namespace ToDoApp.Domain.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        ITaskRepository Tasks { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
