using Tasker.Api.Entities;
using Tasker.Api.Repositories.Interfaces;

namespace Tasker.Api.Repositories;

public class TaskRepository : ITaskRepository
{
    private static readonly List<User> _users;
    private static readonly List<TaskItem> _tasks;
    private static readonly List<Subtask> _subtasks;

    static TaskRepository()
    {
        _users =
        [
            new User { Id = "u-101", Name = "Alice Developer" },
            new User { Id = "u-102", Name = "Bob Engineer" }
        ];

        _tasks =
        [
            new TaskItem 
            { 
                Id = "t-1", 
                Title = "Build API Authentication", 
                Description = "Implement JWT...", 
                Status = "InProgress", 
                AssigneeId = "u-101", 
                CreatedAt = DateTime.Parse("2026-03-25T08:00:00Z").ToUniversalTime() 
            },
            new TaskItem 
            { 
                Id = "t-2", 
                Title = "Write Unit Tests", 
                Description = "Achieve 80%...", 
                Status = "Todo", 
                AssigneeId = null, 
                CreatedAt = DateTime.Parse("2026-03-28T10:30:00Z").ToUniversalTime() 
            }
        ];

        _subtasks =
        [
            new Subtask { Id = "st-1", TaskId = "t-1", Title = "Create User Login POST route", Status = "Done" },
            new Subtask { Id = "st-2", TaskId = "t-1", Title = "Generate JWT Token", Status = "InProgress" }
        ];
    }

    public Task<IEnumerable<TaskItem>> GetAllTasksAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<TaskItem>>(_tasks.ToList());
    }

    public Task<TaskItem?> GetTaskByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        
        // Link subtasks statically since we're using a mock
        if (task != null)
        {
            task.Subtasks = _subtasks.Where(s => s.TaskId == id).ToList();
        }

        return Task.FromResult(task);
    }

    public Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        _tasks.Add(task);
        return Task.FromResult(task);
    }

    public Task<TaskItem?> UpdateTaskAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
        if (existingTask == null) return Task.FromResult<TaskItem?>(null);

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.Status = task.Status;
        existingTask.AssigneeId = task.AssigneeId;
        
        return Task.FromResult<TaskItem?>(existingTask);
    }

    public Task<bool> DeleteTaskAsync(string id, CancellationToken cancellationToken = default)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return Task.FromResult(false);

        _tasks.Remove(task);
        _subtasks.RemoveAll(s => s.TaskId == id); // Cascade delete

        return Task.FromResult(true);
    }

    public Task<TaskItem?> AssignTaskAsync(string taskId, string? userId, CancellationToken cancellationToken = default)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) return Task.FromResult<TaskItem?>(null);

        var userExists = userId == null || _users.Any(u => u.Id == userId);
        if (!userExists) return Task.FromResult<TaskItem?>(null);

        task.AssigneeId = userId;
        return Task.FromResult<TaskItem?>(task);
    }

    public Task<IEnumerable<Subtask>> GetSubtasksAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var subtasks = _subtasks.Where(s => s.TaskId == taskId).ToList();
        return Task.FromResult<IEnumerable<Subtask>>(subtasks);
    }

    public Task<Subtask?> CreateSubtaskAsync(Subtask subtask, CancellationToken cancellationToken = default)
    {
        var taskExists = _tasks.Any(t => t.Id == subtask.TaskId);
        if (!taskExists) return Task.FromResult<Subtask?>(null);

        _subtasks.Add(subtask);
        return Task.FromResult<Subtask?>(subtask);
    }

    public Task<Subtask?> UpdateSubtaskAsync(Subtask subtask, CancellationToken cancellationToken = default)
    {
        var existingSubtask = _subtasks.FirstOrDefault(s => s.Id == subtask.Id);
        if (existingSubtask == null) return Task.FromResult<Subtask?>(null);

        existingSubtask.Title = subtask.Title;
        existingSubtask.Status = subtask.Status;
        
        return Task.FromResult<Subtask?>(existingSubtask);
    }

    public Task<bool> DeleteSubtaskAsync(string subtaskId, CancellationToken cancellationToken = default)
    {
        var subtask = _subtasks.FirstOrDefault(s => s.Id == subtaskId);
        if (subtask == null) return Task.FromResult(false);

        _subtasks.Remove(subtask);
        return Task.FromResult(true);
    }

    public Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var exists = _users.Any(u => u.Id == userId);
        return Task.FromResult(exists);
    }
}
