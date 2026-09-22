using CinemaMS.Application.DTOs;
using CinemaMS.Application.Interfaces.Services;
using CinemaMS.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace CinemaMS.Infrastructure.Services.VnPay;

public class VnPayService : IVnPayService
{
    private readonly VnPaySettings _config;

    public VnPayService(IOptions<VnPaySettings> config)
    {
        _config = config.Value;
    }

    public string CreatePaymentUrl(PaymentInformationModel model, string clientIpAddress)
    {
        var vnpay = new VnPayLibrary();
        
        vnpay.AddRequestData("vnp_Version", _config.Version);
        vnpay.AddRequestData("vnp_Command", _config.Command);
        vnpay.AddRequestData("vnp_TmnCode", _config.TmnCode);
        vnpay.AddRequestData("vnp_Amount", ((long)model.Amount * 100).ToString());
        vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
        vnpay.AddRequestData("vnp_CurrCode", _config.CurrCode);
        vnpay.AddRequestData("vnp_IpAddr", clientIpAddress);
        vnpay.AddRequestData("vnp_Locale", _config.Locale);
        
        vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {model.BookingId}");
        vnpay.AddRequestData("vnp_OrderType", "other");
        vnpay.AddRequestData("vnp_ReturnUrl", _config.ReturnUrl);
        vnpay.AddRequestData("vnp_TxnRef", $"{model.BookingId}_{DateTime.Now.Ticks}");

        var paymentUrl = vnpay.CreateRequestUrl(_config.BaseUrl, _config.HashSecret);
        
        return paymentUrl;
    }

    public PaymentResponseModel PaymentExecute(IDictionary<string, string> collections)
    {
        var vnpay = new VnPayLibrary();
        
        foreach (var (key, value) in collections)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
            {
                vnpay.AddResponseData(key, value);
            }
        }

        var txnRef = vnpay.GetResponseData("vnp_TxnRef");
        var vnp_orderId = Convert.ToInt64(txnRef.Split('_')[0]);
        var vnp_TransactionId = vnpay.GetResponseData("vnp_TransactionNo");
        var vnp_SecureHash = collections.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value;
        var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
        var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

        bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config.HashSecret);

        if (!checkSignature)
        {
            return new PaymentResponseModel
            {
                Success = false
            };
        }

        return new PaymentResponseModel
        {
            Success = vnp_ResponseCode == "00",
            PaymentMethod = "VnPay",
            OrderDescription = vnp_OrderInfo,
            BookingId = (int)vnp_orderId,
            TransactionId = vnp_TransactionId,
            PaymentId = vnp_TransactionId,
            VnPayResponseCode = vnp_ResponseCode
        };
    }
}
