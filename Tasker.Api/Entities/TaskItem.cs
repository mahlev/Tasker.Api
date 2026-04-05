namespace Tasker.Api.Entities;

public class TaskItem
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "Todo";
    public string? AssigneeId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Subtask> Subtasks { get; set; } = new List<Subtask>();
}
