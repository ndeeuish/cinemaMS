using CinemaMS.Application.Features.Showtimes.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Repositories;
using CinemaMS.Application.Interfaces.Caching;

namespace CinemaMS.Application.Features.Showtimes.Queries;

public class GetShowtimesByMovieIdQueryHandler : QueryHandlerBase<GetShowtimesByMovieIdQuery, IEnumerable<ShowtimeDto>>
{
    private readonly IShowtimeRepository _showtimeRepository;
    private readonly IRedisCacheService _cacheService;

    public GetShowtimesByMovieIdQueryHandler(IShowtimeRepository showtimeRepository, IRedisCacheService cacheService)
    {
        _showtimeRepository = showtimeRepository;
        _cacheService = cacheService;
    }

    public override async Task<IEnumerable<ShowtimeDto>> Handle(GetShowtimesByMovieIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"Showtimes_Movie_{request.MovieId}";
        
        var cachedData = await _cacheService.GetAsync<IEnumerable<ShowtimeDto>>(cacheKey, cancellationToken);
        if (cachedData != null) return cachedData;

        var showtimes = await _showtimeRepository.GetByMovieIdAsync(request.MovieId, cancellationToken);
        var result = showtimes.Select(ShowtimeDto.FromEntity).ToList();

        await _cacheService.SetAsync(cacheKey, result, absoluteExpireTime: TimeSpan.FromMinutes(10), cancellationToken: cancellationToken);

        return result;
    }
}
