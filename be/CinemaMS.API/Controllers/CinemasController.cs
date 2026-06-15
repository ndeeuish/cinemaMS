using CinemaMS.Application.Features.Cinemas.Commands;
using CinemaMS.Application.Features.Cinemas.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CinemasController : ControllerBase
{
    private readonly IMediator _mediator;

    public CinemasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Pagging-cinema")]
    public async Task<IActionResult> GetCinemas([FromQuery] GetCinemasQuery query) 
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("Get-cinema/{id}")]
    public async Task<IActionResult> GetCinemaById(int id)
    {
        var result = await _mediator.Send(new GetCinemaByIdQuery(id));
        return Ok(result);
    }

    [HttpPost("Create-cinema")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCinema([FromBody] CreateCinemaCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCinemaById), new { id = result.Id }, result);
    }

    [HttpPut("Update-cinema/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCinema(int id, [FromBody] UpdateCinemaCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("Delete-cinema/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCinema(int id)
    {
        var result = await _mediator.Send(new DeleteCinemaCommand(id));
        return Ok(new { message = "Deleted successfully", deletedId = result });
    }
}
