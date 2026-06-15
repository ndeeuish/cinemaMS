namespace CinemaMS.Application.Features.Common.Pagination;

public class FilterBase
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Keyword { get; set; }

    public int SkipCount => (PageIndex - 1) * PageSize;
}
