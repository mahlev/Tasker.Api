namespace Tasker.Api.Dtos.Tasks;

using System.ComponentModel.DataAnnotations;

public class TaskCreateDto
{
    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }

    public string? Description { get; set; }
}
