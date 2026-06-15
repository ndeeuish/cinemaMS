using CinemaMS.Application.Interfaces.Caching;
using CinemaMS.Application.Interfaces.Services;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Auth.Commands;

public class ForgotPasswordCommandHandler : CommandHandlerBase<ForgotPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IRedisCacheService _cacheService;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository, 
        IRedisCacheService cacheService, 
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _cacheService = cacheService;
        _emailService = emailService;
    }

    public override async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            // Always return true to prevent email enumeration attacks
            return true;
        }

        var cooldownKey = $"Cooldown_{request.Email}";
        var isCooldown = await _cacheService.GetAsync<string>(cooldownKey, cancellationToken);
        if (isCooldown != null)
        {
            throw new UserFriendlyException("Please wait 60 seconds before requesting a new OTP.");
        }

        // Generate 6-digit OTP
        var otp = new Random().Next(100000, 999999).ToString();
        var otpKey = $"OTP_{request.Email}";

        // Save to Redis: OTP expires in 5 minutes, Cooldown expires in 60 seconds
        await _cacheService.SetAsync(otpKey, otp, absoluteExpireTime: TimeSpan.FromMinutes(5), cancellationToken: cancellationToken);
        await _cacheService.SetAsync(cooldownKey, "locked", absoluteExpireTime: TimeSpan.FromSeconds(60), cancellationToken: cancellationToken);

        // Send Email
        var subject = "CinemaMS - Password Reset OTP";
        var body = $@"
            <h2>Password Reset Request</h2>
            <p>Hi {user.FullName},</p>
            <p>You requested to reset your password. Here is your 6-digit OTP:</p>
            <h1 style='color: #d9534f; letter-spacing: 5px;'>{otp}</h1>
            <p>This code will expire in 5 minutes.</p>
            <p>If you did not request this, please ignore this email.</p>
        ";

        await _emailService.SendEmailAsync(request.Email, subject, body, cancellationToken);

        return true;
    }
}
