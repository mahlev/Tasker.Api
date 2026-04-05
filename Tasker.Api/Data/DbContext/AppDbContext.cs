using Microsoft.EntityFrameworkCore;
using Tasker.Api.Entities;

namespace Tasker.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<Subtask> Subtasks => Set<Subtask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasMany(e => e.Subtasks)
                  .WithOne(e => e.TaskItem)
                  .HasForeignKey(e => e.TaskId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Subtask>(entity =>
        {
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.TaskId).IsRequired();
        });
    }
}
