namespace Tasker.Api.Entities;

public class Subtask
{
    public required string Id { get; set; }
    public required string TaskId { get; set; }
    public required string Title { get; set; }
    public string? Status { get; set; }

    public TaskItem? TaskItem { get; set; }
}
