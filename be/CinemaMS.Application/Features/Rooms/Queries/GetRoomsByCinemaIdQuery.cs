using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Application.Features.Rooms.DTOs;
using MediatR;

namespace CinemaMS.Application.Features.Rooms.Queries;

public class GetRoomsByCinemaIdQuery : FilterBase, IRequest<PagedResult<RoomDto>>
{
    public int CinemaId { get; set; }

    public GetRoomsByCinemaIdQuery(int cinemaId)
    {
        CinemaId = cinemaId;
    }
    
    // For model binding from query string
    public GetRoomsByCinemaIdQuery() { }
}
