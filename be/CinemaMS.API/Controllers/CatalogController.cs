using CinemaMS.Application.Features.Catalog.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("genres")]
    public async Task<IActionResult> GetGenres()
    {
        var result = await _mediator.Send(new GetAllGenresQuery());
        return Ok(result);
    }

    [HttpGet("age-restrictions")]
    public async Task<IActionResult> GetAgeRestrictions()
    {
        var result = await _mediator.Send(new GetAllAgeRestrictionsQuery());
        return Ok(result);
    }

    [HttpGet("room-types")]
    public async Task<IActionResult> GetRoomTypes()
    {
        var result = await _mediator.Send(new GetAllRoomTypesQuery());
        return Ok(result);
    }

    [HttpGet("seat-types")]
    public async Task<IActionResult> GetSeatTypes()
    {
        var result = await _mediator.Send(new GetAllSeatTypesQuery());
        return Ok(result);
    }
}
