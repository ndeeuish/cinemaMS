using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Showtimes.Commands;

public class DeleteShowtimeCommand : CommandBase<int>
{
    public int Id { get; set; }
    public DeleteShowtimeCommand(int id) => Id = id;
}
