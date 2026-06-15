using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Rooms.Commands;

public class DeleteRoomCommand : CommandBase<int>
{
    public int Id { get; set; }
    public DeleteRoomCommand(int id) => Id = id;
}
