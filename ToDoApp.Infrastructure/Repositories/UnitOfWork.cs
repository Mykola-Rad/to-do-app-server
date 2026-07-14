using ToDoApp.Domain.Repositories;
using ToDoApp.Infrastructure.Data;

namespace ToDoApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ToDoDbContext _context;

        public IUserRepository Users { get; }
        public ICategoryRepository Categories { get; }
        public ITaskRepository Tasks { get; }

        public UnitOfWork(ToDoDbContext context)
        {
            _context = context;

            Users = new UserRepository(_context);
            Categories = new CategoryRepository(_context);
            Tasks = new TaskRepository(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
