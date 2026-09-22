namespace CinemaMS.Application.DTOs;

public class PaymentInformationModel
{
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public string OrderDescription { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class PaymentResponseModel
{
    public string OrderDescription { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public int BookingId { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string VnPayResponseCode { get; set; } = string.Empty;
}
