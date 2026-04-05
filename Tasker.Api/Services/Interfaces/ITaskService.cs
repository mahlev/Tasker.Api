namespace Tasker.Api.Services.Interfaces;

using Tasker.Api.Dtos.Tasks;

public interface ITaskService
{
    Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync(CancellationToken cancellationToken = default);
    Task<TaskResponseDto> GetTaskByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<TaskResponseDto> CreateTaskAsync(TaskCreateDto dto, CancellationToken cancellationToken = default);
    Task<TaskResponseDto> UpdateTaskAsync(string id, TaskUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteTaskAsync(string id, CancellationToken cancellationToken = default);
    Task<TaskResponseDto> AssignTaskAsync(string id, TaskAssignDto dto, CancellationToken cancellationToken = default);

    Task<IEnumerable<SubtaskResponseDto>> GetSubtasksAsync(string taskId, CancellationToken cancellationToken = default);
    Task<SubtaskResponseDto> CreateSubtaskAsync(string taskId, SubtaskCreateDto dto, CancellationToken cancellationToken = default);
    Task<SubtaskResponseDto> UpdateSubtaskAsync(string subtaskId, SubtaskCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteSubtaskAsync(string subtaskId, CancellationToken cancellationToken = default);
}
