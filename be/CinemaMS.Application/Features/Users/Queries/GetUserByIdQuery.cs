using CinemaMS.Application.Features.Users.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Users.Queries;

public class GetUserByIdQuery : QueryBase<UserDto>
{
    public int Id { get; set; }
    
    public GetUserByIdQuery(int id)
    {
        Id = id;
    }
}
