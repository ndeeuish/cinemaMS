using CinemaMS.Application.Features.Rooms.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Domain.Repositories;
using MediatR;

namespace CinemaMS.Application.Features.Rooms.Queries;

public class GetRoomsByCinemaIdQueryHandler : IRequestHandler<GetRoomsByCinemaIdQuery, PagedResult<RoomDto>>
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomsByCinemaIdQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<PagedResult<RoomDto>> Handle(GetRoomsByCinemaIdQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _roomRepository.GetPagedByCinemaIdAsync(
            request.CinemaId,
            request.PageIndex,
            request.PageSize,
            request.Keyword,
            cancellationToken);

        return new PagedResult<RoomDto>
        {
            Items = items.Select(RoomDto.FromEntity),
            TotalItems = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
