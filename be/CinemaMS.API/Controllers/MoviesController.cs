using CinemaMS.Application.Features.Movies.Commands;
using CinemaMS.Application.Features.Movies.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Pagging-movie")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllMoviesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("Get-movie/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetMovieByIdQuery(id));
        return Ok(result);
    }

    [HttpPost("Create-movie")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Create([FromBody] CreateMovieCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("Update-movie/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMovieCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("Delete-movie/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedId = await _mediator.Send(new DeleteMovieCommand(id));
        return Ok(new { message = "Movie deleted successfully", id = deletedId });
    }
}
