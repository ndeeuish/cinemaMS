using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Movies.Commands;

public class DeleteMovieCommand : CommandBase<int>
{
    public int Id { get; set; }
    public DeleteMovieCommand(int id) => Id = id;
}
