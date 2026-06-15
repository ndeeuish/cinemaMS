using CinemaMS.Application.Interfaces.Caching;
using CinemaMS.Application.Interfaces.Services;
using CinemaMS.Application.Interfaces.Security;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Auth.Commands;

public class ResetPasswordCommandHandler : CommandHandlerBase<ResetPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IRedisCacheService _cacheService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository, 
        IRedisCacheService cacheService, 
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _cacheService = cacheService;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public override async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var otpKey = $"OTP_{request.Email}";
        var savedOtp = await _cacheService.GetAsync<string>(otpKey, cancellationToken);

        if (string.IsNullOrEmpty(savedOtp) || savedOtp != request.Otp)
        {
            throw new UserFriendlyException("Invalid or expired OTP.");
        }

        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new UserFriendlyException("User not found.");
        }

        // Update password
        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Invalidate OTP after successful use
        await _cacheService.RemoveAsync(otpKey, cancellationToken);

        return true;
    }
}
