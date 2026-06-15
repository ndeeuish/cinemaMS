using CinemaMS.Application.Features.Cinemas.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Cinemas.Queries;

public class GetCinemaByIdQuery : QueryBase<CinemaDto>
{
    public int Id { get; set; }
    public GetCinemaByIdQuery(int id) { Id = id; }
}
