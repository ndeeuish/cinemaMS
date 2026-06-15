using CinemaMS.Application.Features.Dashboards.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMS.API.Controllers;

[ApiController]
[Route("api/dashboards")]
[Authorize(Roles = "Admin")]
public class DashboardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview([FromQuery] GetDashboardOverviewQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("top-movies")]
    public async Task<IActionResult> GetTopMovies([FromQuery] GetTopMoviesRevenueQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
