namespace Tasker.Api.Dtos.Tasks;

public class SubtaskResponseDto
{
    public required string Id { get; set; }
    public required string TaskId { get; set; }
    public required string Title { get; set; }
    public string? Status { get; set; }
}
