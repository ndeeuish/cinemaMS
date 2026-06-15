using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Application.Features.Users.DTOs;
using MediatR;

namespace CinemaMS.Application.Features.Users.Queries;

public class GetAllUsersQuery : FilterBase, IRequest<PagedResult<UserDto>>
{
    public int? RoleId { get; set; }
}
