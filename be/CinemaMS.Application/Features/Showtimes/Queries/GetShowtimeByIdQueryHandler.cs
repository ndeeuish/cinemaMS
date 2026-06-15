using CinemaMS.Application.Features.Showtimes.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Showtimes.Queries;

public class GetShowtimeByIdQueryHandler : QueryHandlerBase<GetShowtimeByIdQuery, ShowtimeDto>
{
    private readonly IShowtimeRepository _showtimeRepository;

    public GetShowtimeByIdQueryHandler(IShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }

    public override async Task<ShowtimeDto> Handle(GetShowtimeByIdQuery request, CancellationToken cancellationToken)
    {
        var showtime = await _showtimeRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (showtime == null)
            throw new NotFoundException("Showtime", request.Id);

        return ShowtimeDto.FromEntity(showtime);
    }
}
