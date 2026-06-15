using CinemaMS.Application.Features.Seats.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Seats.Queries;

public class GetSeatsByRoomIdQueryHandler : QueryHandlerBase<GetSeatsByRoomIdQuery, IEnumerable<SeatDto>>
{
    private readonly ISeatRepository _seatRepository;

    public GetSeatsByRoomIdQueryHandler(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }

    public override async Task<IEnumerable<SeatDto>> Handle(GetSeatsByRoomIdQuery request, CancellationToken cancellationToken)
    {
        var seats = await _seatRepository.GetSeatsByRoomIdAsync(request.RoomId, cancellationToken);
        return seats.Select(SeatDto.FromEntity);
    }
}
