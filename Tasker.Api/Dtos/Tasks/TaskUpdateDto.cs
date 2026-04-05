namespace Tasker.Api.Dtos.Tasks;

using System.ComponentModel.DataAnnotations;

public class TaskUpdateDto
{
    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }

    public string? Description { get; set; }

    [Required]
    public required string Status { get; set; }
}
