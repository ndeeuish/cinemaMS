using CinemaMS.Application.Features.Rooms.Commands;
using CinemaMS.Application.Features.Rooms.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Get-room/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetRoomByIdQuery(id));
        return Ok(result);
    }

    [HttpGet("Get-room-by-cinema/{cinemaId}")]
    public async Task<IActionResult> GetByCinema(int cinemaId, [FromQuery] GetRoomsByCinemaIdQuery query)
    {
        query.CinemaId = cinemaId;
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("Create-room")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateRoomCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("Update-room/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("Delete-room/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedId = await _mediator.Send(new DeleteRoomCommand(id));
        return Ok(new { message = "Room deleted successfully", id = deletedId });
    }
}
