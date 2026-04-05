using Tasker.Api.Entities;

namespace Tasker.Api.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllTasksAsync(CancellationToken cancellationToken = default);
    Task<TaskItem?> GetTaskByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken = default);
    Task<TaskItem?> UpdateTaskAsync(TaskItem task, CancellationToken cancellationToken = default);
    Task<bool> DeleteTaskAsync(string id, CancellationToken cancellationToken = default);
    Task<TaskItem?> AssignTaskAsync(string taskId, string? userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Subtask>> GetSubtasksAsync(string taskId, CancellationToken cancellationToken = default);
    Task<Subtask?> CreateSubtaskAsync(Subtask subtask, CancellationToken cancellationToken = default);
    Task<Subtask?> UpdateSubtaskAsync(Subtask subtask, CancellationToken cancellationToken = default);
    Task<bool> DeleteSubtaskAsync(string subtaskId, CancellationToken cancellationToken = default);

    Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default);
}
