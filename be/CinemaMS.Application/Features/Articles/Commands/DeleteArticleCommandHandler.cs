using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Repositories;
using MediatR;

namespace CinemaMS.Application.Features.Articles.Commands;

public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, int>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (article == null)
            throw new NotFoundException(nameof(Article), request.Id);

        _articleRepository.Delete(article);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}
