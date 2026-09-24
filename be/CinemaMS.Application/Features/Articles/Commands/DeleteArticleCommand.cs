using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Articles.Commands;

public class DeleteArticleCommand : CommandBase<int>
{
    public int Id { get; set; }
    public DeleteArticleCommand(int id) => Id = id;
}
