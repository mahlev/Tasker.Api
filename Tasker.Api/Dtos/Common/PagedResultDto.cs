namespace Tasker.Api.Dtos.Common;

public class PagedResultDto<T>
{
    public required IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
}
