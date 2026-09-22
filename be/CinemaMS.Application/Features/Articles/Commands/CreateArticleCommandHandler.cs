using CinemaMS.Application.Features.Articles.DTOs;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Repositories;
using MediatR;

namespace CinemaMS.Application.Features.Articles.Commands;

public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, ArticleDto>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ArticleDto> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = new Article
        {
            Title = request.Title,
            Summary = request.Summary,
            Content = request.Content,
            ImageUrl = request.ImageUrl,
            Type = request.Type,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = request.IsActive
        };

        _articleRepository.Add(article);
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
