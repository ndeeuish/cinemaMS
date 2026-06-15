using CinemaMS.Application.Features.Showtimes.Commands;
using CinemaMS.Application.Features.Showtimes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowtimesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShowtimesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Get-showtime-by-movie/{movieId}")]
    public async Task<IActionResult> GetByMovie(int movieId)
    {
        var result = await _mediator.Send(new GetShowtimesByMovieIdQuery(movieId));
        return Ok(result);
    }

    [HttpGet("Get-showtime/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetShowtimeByIdQuery(id));
        return Ok(result);
    }

    [HttpGet("Get-showtime-by-cinema/{cinemaId}")]
    public async Task<IActionResult> GetByCinema(int cinemaId, [FromQuery] GetShowtimesByCinemaIdQuery query)
    {
        query.CinemaId = cinemaId;
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("Create-showtime")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Create([FromBody] CreateShowtimeCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("Update-showtime/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateShowtimeCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("Delete-showtime/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedId = await _mediator.Send(new DeleteShowtimeCommand(id));
        return Ok(new { message = "Showtime deleted successfully", id = deletedId });
    }
}
