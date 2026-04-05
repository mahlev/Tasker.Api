namespace Tasker.Api.Dtos.Tasks;

public class TaskResponseDto
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Status { get; set; }
    public string? AssigneeId { get; set; }
    public int SubtaskCount { get; set; }
}
