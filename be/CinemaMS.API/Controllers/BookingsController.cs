using CinemaMS.Application.Features.Bookings.Commands;
using CinemaMS.Application.Features.Bookings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CinemaMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Get-all-bookings")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllBookingsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("showtime/{showtimeId}/get-reserved-seats")]
    public async Task<IActionResult> GetReservedSeats(int showtimeId)
    {
        var result = await _mediator.Send(new GetReservedSeatsQuery(showtimeId));
        return Ok(result);
    }

    [HttpGet("get-my-bookings")]
    [Authorize]
    public async Task<IActionResult> GetMyBookings()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

        var userId = int.Parse(userIdString);
        var result = await _mediator.Send(new GetMyBookingsQuery(userId));
        return Ok(result);
    }

    [HttpPost("Create-booking")]
    [Authorize]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

        command.UserId = int.Parse(userIdString);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/confirm-payment")]
    [Authorize]
    public async Task<IActionResult> ConfirmPayment(int id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

        var command = new ConfirmBookingCommand
        {
            BookingId = id,
            UserId = int.Parse(userIdString)
        };
        
        var result = await _mediator.Send(command);
        return Ok(new { success = result, message = "Payment confirmed and booking finalized." });
    }
}
