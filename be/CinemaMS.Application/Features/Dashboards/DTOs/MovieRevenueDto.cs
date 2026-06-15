namespace CinemaMS.Application.Features.Dashboards.DTOs;

public class MovieRevenueDto
{
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = default!;
    public int TicketsSold { get; set; }
    public decimal Revenue { get; set; }
}
