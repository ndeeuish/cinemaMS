using CinemaMS.Application.Features.Dashboards.DTOs;
using CinemaMS.Application.Interfaces.Data;
using CinemaMS.Application.Messaging;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Application.Features.Dashboards.Queries;

public class GetDashboardOverviewQueryHandler : QueryHandlerBase<GetDashboardOverviewQuery, DashboardOverviewDto>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardOverviewQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public override async Task<DashboardOverviewDto> Handle(GetDashboardOverviewQuery request, CancellationToken cancellationToken)
    {
        // 1. Calculate Total Users (Created within date range)
        var usersQuery = _context.Users.AsNoTracking().Where(u => !u.IsDeleted);
        if (request.FromDate.HasValue) usersQuery = usersQuery.Where(u => u.CreatedAt >= request.FromDate.Value);
        if (request.ToDate.HasValue) usersQuery = usersQuery.Where(u => u.CreatedAt <= request.ToDate.Value);
        var totalUsers = await usersQuery.CountAsync(cancellationToken);

        // 2. Base Ticket Query for Revenue and Tickets Sold
        var ticketsQuery = _context.Tickets.AsNoTracking()
            .Include(t => t.Booking)
            .Include(t => t.Showtime)
            .ThenInclude(s => s.Room)
            .Where(t => t.Booking.Status == "Confirmed");

        // Apply Filters
        if (request.FromDate.HasValue) ticketsQuery = ticketsQuery.Where(t => t.CreatedAt >= request.FromDate.Value);
        if (request.ToDate.HasValue) ticketsQuery = ticketsQuery.Where(t => t.CreatedAt <= request.ToDate.Value);
        if (request.CinemaId.HasValue) ticketsQuery = ticketsQuery.Where(t => t.Showtime.Room.CinemaId == request.CinemaId.Value);
        if (request.MovieId.HasValue) ticketsQuery = ticketsQuery.Where(t => t.Showtime.MovieId == request.MovieId.Value);

        var ticketsData = await ticketsQuery
            .Select(t => t.Price)
            .ToListAsync(cancellationToken);

        var totalTicketsSold = ticketsData.Count;
        var totalRevenue = ticketsData.Sum();

        // 3. Calculate Total Movies
        var moviesQuery = _context.Movies.AsNoTracking().Where(m => !m.IsDeleted);
        // If we want to strictly count movies that had showtimes in this period/cinema:
        if (request.FromDate.HasValue || request.ToDate.HasValue || request.CinemaId.HasValue)
        {
            var showtimesQuery = _context.Showtimes.AsNoTracking().Include(s => s.Room).AsQueryable();
            if (request.FromDate.HasValue) showtimesQuery = showtimesQuery.Where(s => s.StartTime >= request.FromDate.Value);
            if (request.ToDate.HasValue) showtimesQuery = showtimesQuery.Where(s => s.StartTime <= request.ToDate.Value);
            if (request.CinemaId.HasValue) showtimesQuery = showtimesQuery.Where(s => s.Room.CinemaId == request.CinemaId.Value);
            
            var validMovieIds = await showtimesQuery.Select(s => s.MovieId).Distinct().ToListAsync(cancellationToken);
            moviesQuery = moviesQuery.Where(m => validMovieIds.Contains(m.Id));
        }

        if (request.MovieId.HasValue)
        {
            moviesQuery = moviesQuery.Where(m => m.Id == request.MovieId.Value);
        }

        var totalMovies = await moviesQuery.CountAsync(cancellationToken);

        return new DashboardOverviewDto
        {
            TotalUsers = totalUsers,
            TotalTicketsSold = totalTicketsSold,
            TotalRevenue = totalRevenue,
            TotalMovies = totalMovies
        };
    }
}
