using CinemaMS.Application.Features.Showtimes.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Showtimes.Queries;

public class GetShowtimeByIdQuery : QueryBase<ShowtimeDto>
{
    public int Id { get; set; }

    public GetShowtimeByIdQuery(int id)
    {
        Id = id;
    }
}
