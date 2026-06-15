using CinemaMS.Application.Features.Showtimes.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Bookings;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Entities.Cinemas;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Showtimes.Commands;

public class CreateShowtimeCommandHandler : CommandHandlerBase<CreateShowtimeCommand, ShowtimeDto>
{
    private readonly IShowtimeRepository _showtimeRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateShowtimeCommandHandler(
        IShowtimeRepository showtimeRepository,
        IMovieRepository movieRepository,
        IRoomRepository roomRepository,
        IUnitOfWork unitOfWork)
    {
        _showtimeRepository = showtimeRepository;
        _movieRepository = movieRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<ShowtimeDto> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId, cancellationToken);
        if (movie == null) throw new NotFoundException(nameof(Movie), request.MovieId);

        var room = await _roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room == null) throw new NotFoundException(nameof(Room), request.RoomId);

        // Auto-calculate EndTime = StartTime + Duration + 30 mins (prep time)
        var estimatedEndTime = request.StartTime.AddMinutes(movie.DurationInMinutes + 30);

        // Validate overlapping
        var isOverlapping = await _showtimeRepository.HasOverlappingShowtimeAsync(
            request.RoomId, 
            request.StartTime, 
            estimatedEndTime, 
            null, 
            cancellationToken);

        if (isOverlapping)
            throw new UserFriendlyException($"Scheduling failed. The room is already booked for another showtime between {request.StartTime} and {estimatedEndTime}.");

        var showtime = new Showtime
        {
            MovieId = request.MovieId,
            RoomId = request.RoomId,
            StartTime = request.StartTime,
            EndTime = estimatedEndTime,
            BasePrice = request.BasePrice
        };

        _showtimeRepository.Add(showtime);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map DTO
        showtime.Movie = movie;
        showtime.Room = room;
        return ShowtimeDto.FromEntity(showtime);
    }
}
