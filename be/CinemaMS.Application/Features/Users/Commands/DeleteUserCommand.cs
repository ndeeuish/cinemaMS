using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Users.Commands;

public class DeleteUserCommand : CommandBase<bool>
{
    public int Id { get; set; }

    public DeleteUserCommand(int id)
    {
        Id = id;
    }
}
