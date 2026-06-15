using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Cinemas.Commands;

public class DeleteCinemaCommand : CommandBase<int>
{
    public int Id { get; set; }
    public DeleteCinemaCommand(int id) { Id = id; }
}
