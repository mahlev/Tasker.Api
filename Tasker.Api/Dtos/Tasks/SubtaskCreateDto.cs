namespace Tasker.Api.Dtos.Tasks;

using System.ComponentModel.DataAnnotations;

public class SubtaskCreateDto
{
    [Required]
    public required string Title { get; set; }
}
