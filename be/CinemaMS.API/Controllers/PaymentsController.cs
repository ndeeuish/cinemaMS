using CinemaMS.Application.DTOs;
using CinemaMS.Application.Interfaces.Services;
using CinemaMS.Application.Features.Payments.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IVnPayService _vnPayService;
    private readonly IMediator _mediator;
    private readonly IBookingRepository _bookingRepository;
    private readonly IConfiguration _configuration;

    public PaymentsController(IVnPayService vnPayService, IMediator mediator, IBookingRepository bookingRepository, IConfiguration configuration)
    {
        _vnPayService = vnPayService;
        _mediator = mediator;
        _bookingRepository = bookingRepository;
        _configuration = configuration;
    }

    [HttpPost("create-url")]
    [Authorize]
    public async Task<IActionResult> CreatePaymentUrl([FromBody] int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, CancellationToken.None);
        if (booking == null) return NotFound("Booking not found");

        if (booking.Status != "Holding")
            return BadRequest(new { message = "Booking is not in Holding status or already paid." });

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";

        var model = new PaymentInformationModel
        {
            BookingId = bookingId,
            Amount = booking.TotalAmount,
            Name = "Thanh toan VNPAY",
            OrderDescription = $"Thanh toan ve xem phim ma {bookingId}"
        };

        var url = _vnPayService.CreatePaymentUrl(model, ipAddress);

        return Ok(new { url });
    }

    [HttpGet("vnpay-return")]
    public async Task<IActionResult> PaymentCallback()
    {
        var response = _vnPayService.PaymentExecute(Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString()));

        string frontendBaseUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
        string frontendUrl = $"{frontendBaseUrl.TrimEnd('/')}/checkout/result";
        
        if (response.Success)
        {
            try
            {
                var command = new ProcessPaymentCommand 
                { 
                    BookingId = response.BookingId, 
                    PaymentMethod = "VnPay", 
                    TransactionCode = response.TransactionId 
                };
                await _mediator.Send(command);
                
                return Redirect($"{frontendUrl}?status=success&bookingId={response.BookingId}&txnId={response.TransactionId}");
            }
            catch (Exception ex)
            {
                return Redirect($"{frontendUrl}?status=failed&message={Uri.EscapeDataString(ex.Message)}");
            }
        }

        return Redirect($"{frontendUrl}?status=failed&message={Uri.EscapeDataString("Giao dịch bị huỷ hoặc thất bại từ VNPAY.")}");
    }
}
