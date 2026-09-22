using CinemaMS.Domain.Exceptions;
using CinemaMS.Application.Features.Articles.DTOs;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Repositories;
using MediatR;

namespace CinemaMS.Application.Features.Articles.Commands;

public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, ArticleDto>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ArticleDto> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (article == null)
            throw new NotFoundException(nameof(Article), request.Id);

        article.Title = request.Title;
        article.Summary = request.Summary;
        article.Content = request.Content;
        article.ImageUrl = request.ImageUrl;
        article.Type = request.Type;
        article.StartDate = request.StartDate;
        article.EndDate = request.EndDate;
        article.IsActive = request.IsActive;

        _articleRepository.Update(article);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ArticleDto
        {
            Id = article.Id,
            Title = article.Title,
            Summary = article.Summary,
            Content = article.Content,
            ImageUrl = article.ImageUrl,
            Type = article.Type,
            StartDate = article.StartDate,
            EndDate = article.EndDate,
            IsActive = article.IsActive,
            CreatedAt = article.CreatedAt
        };
    }
}
