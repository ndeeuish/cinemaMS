using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Application.Features.Users.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Repositories;
using MediatR;

namespace CinemaMS.Application.Features.Users.Queries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResult<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedResult<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _userRepository.GetPagedWithDetailsAsync(
            request.PageIndex, 
            request.PageSize, 
            request.RoleId, 
            request.Keyword, 
            cancellationToken);

        return new PagedResult<UserDto>
        {
            Items = items.Select(UserDto.FromEntity),
            TotalItems = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
