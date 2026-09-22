using CinemaMS.Application.DTOs;

namespace CinemaMS.Application.Interfaces.Services;

public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, string clientIpAddress);
    PaymentResponseModel PaymentExecute(IDictionary<string, string> collections);
}
