using CinemaMS.Application.Features.Articles.Commands;
using CinemaMS.Application.Features.Articles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArticlesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Pagging-article")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllArticlesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("Get-article/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetArticleByIdQuery(id));
        return Ok(result);
    }

    [HttpPost("Create-article")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Create([FromBody] CreateArticleCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("Update-article/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateArticleCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("Delete-article/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedId = await _mediator.Send(new DeleteArticleCommand(id));
        return Ok(new { message = "Article deleted successfully", id = deletedId });
    }
}
