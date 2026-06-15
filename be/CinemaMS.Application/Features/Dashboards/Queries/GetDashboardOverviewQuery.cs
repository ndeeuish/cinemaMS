using CinemaMS.Application.Features.Dashboards.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Dashboards.Queries;

public class GetDashboardOverviewQuery : QueryBase<DashboardOverviewDto>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? CinemaId { get; set; }
    public int? MovieId { get; set; }
}
