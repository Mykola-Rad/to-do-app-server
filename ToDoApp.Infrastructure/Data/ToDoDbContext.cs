using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastructure.Data;

public class ToDoDbContext : DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ToDoTask> ToDoTasks => Set<ToDoTask>();
    public DbSet<ToDoTaskStep> ToDoTaskSteps => Set<ToDoTaskStep>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(512);

            entity.HasIndex(u => u.Email)
                .IsUnique();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(c => c.UserId);
        });

        modelBuilder.Entity<ToDoTask>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.Description)
                .HasMaxLength(2000);

            entity.Property(t => t.CreatedAt)
                .IsRequired();

            entity.HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(t => new { t.UserId, t.CategoryId, t.IsCompleted });

            entity.HasIndex(t => t.DueDate);
        });

        modelBuilder.Entity<ToDoTaskStep>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasOne(s => s.ToDoTask)
                .WithMany(t => t.Steps)
                .HasForeignKey(s => s.ToDoTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(s => s.ToDoTaskId);
        });
    }
}