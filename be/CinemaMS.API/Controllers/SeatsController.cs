using CinemaMS.Application.Features.Seats.Commands;
using CinemaMS.Application.Features.Seats.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SeatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Get-seat-by-room/{roomId}")]
    public async Task<IActionResult> GetByRoom(int roomId)
    {
        var result = await _mediator.Send(new GetSeatsByRoomIdQuery(roomId));
        return Ok(result);
    }

    [HttpPost("bulk")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBulk([FromBody] CreateBulkSeatsCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { message = $"Successfully generated default seat layout for Room ID {command.RoomId}" });
    }
}
