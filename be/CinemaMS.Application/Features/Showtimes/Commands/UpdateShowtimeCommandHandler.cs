using CinemaMS.Application.Features.Showtimes.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Bookings;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Showtimes.Commands;

public class UpdateShowtimeCommandHandler : CommandHandlerBase<UpdateShowtimeCommand, ShowtimeDto>
{
    private readonly IShowtimeRepository _showtimeRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateShowtimeCommandHandler(IShowtimeRepository showtimeRepository, IMovieRepository movieRepository, IUnitOfWork unitOfWork)
    {
        _showtimeRepository = showtimeRepository;
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<ShowtimeDto> Handle(UpdateShowtimeCommand request, CancellationToken cancellationToken)
    {
        var showtime = await _showtimeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (showtime == null) throw new NotFoundException(nameof(Showtime), request.Id);

        var movie = await _movieRepository.GetByIdAsync(showtime.MovieId, cancellationToken);
        if (movie == null) throw new NotFoundException(nameof(Movie), showtime.MovieId);
        
        var estimatedEndTime = request.StartTime.AddMinutes(movie.DurationInMinutes + 30);

        if (showtime.StartTime != request.StartTime)
        {
            var isOverlapping = await _showtimeRepository.HasOverlappingShowtimeAsync(
                showtime.RoomId, 
                request.StartTime, 
                estimatedEndTime, 
                showtime.Id, 
                cancellationToken);

            if (isOverlapping)
                throw new UserFriendlyException($"Update failed. The room is already booked between {request.StartTime} and {estimatedEndTime}.");
        }

        showtime.StartTime = request.StartTime;
        showtime.EndTime = estimatedEndTime;
        showtime.BasePrice = request.BasePrice;

        _showtimeRepository.Update(showtime);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ShowtimeDto.FromEntity(showtime);
    }
}
