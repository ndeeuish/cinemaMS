using CinemaMS.Application.Features.Cinemas.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Cinemas.Commands;

public class CreateCinemaCommand : CommandBase<CinemaDto>
{
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Hotline { get; set; } = default!;
}
