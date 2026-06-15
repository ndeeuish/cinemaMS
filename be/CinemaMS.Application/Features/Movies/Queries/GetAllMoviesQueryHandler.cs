using CinemaMS.Application.Features.Common.Pagination;
using CinemaMS.Application.Features.Movies.DTOs;
using CinemaMS.Domain.Repositories;
using MediatR;

using CinemaMS.Application.Interfaces.Caching;

namespace CinemaMS.Application.Features.Movies.Queries;

public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, PagedResult<MovieDto>>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IRedisCacheService _cacheService;

    public GetAllMoviesQueryHandler(IMovieRepository movieRepository, IRedisCacheService cacheService)
    {
        _movieRepository = movieRepository;
        _cacheService = cacheService;
    }

    public async Task<PagedResult<MovieDto>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"Movies_{request.PageIndex}_{request.PageSize}_{request.Keyword ?? "all"}_{request.GenreId?.ToString() ?? "all"}";
        
        var cachedResult = await _cacheService.GetAsync<PagedResult<MovieDto>>(cacheKey, cancellationToken);
        if (cachedResult != null) return cachedResult;

        var (items, totalCount) = await _movieRepository.GetPagedWithDetailsAsync(
            request.PageIndex, 
            request.PageSize, 
            request.Keyword,
            request.GenreId,
            cancellationToken);

        var result = new PagedResult<MovieDto>
        {
            Items = items.Select(MovieDto.FromEntity),
            TotalItems = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };

        await _cacheService.SetAsync(cacheKey, result, absoluteExpireTime: TimeSpan.FromMinutes(30), slidingExpireTime: null, cancellationToken);

        return result;
    }
}
