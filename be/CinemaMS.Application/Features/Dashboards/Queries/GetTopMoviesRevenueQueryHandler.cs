using CinemaMS.Application.Features.Dashboards.DTOs;
using CinemaMS.Application.Interfaces.Data;
using CinemaMS.Application.Messaging;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Application.Features.Dashboards.Queries;

public class GetTopMoviesRevenueQueryHandler : QueryHandlerBase<GetTopMoviesRevenueQuery, IEnumerable<MovieRevenueDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTopMoviesRevenueQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<MovieRevenueDto>> Handle(GetTopMoviesRevenueQuery request, CancellationToken cancellationToken)
    {
        var ticketsQuery = _context.Tickets.AsNoTracking()
            .Include(t => t.Booking)
            .Include(t => t.Showtime)
            .ThenInclude(s => s.Room)
            .Include(t => t.Showtime.Movie)
            .Where(t => t.Booking.Status == "Confirmed");

        // Apply Filters
        if (request.FromDate.HasValue) ticketsQuery = ticketsQuery.Where(t => t.CreatedAt >= request.FromDate.Value);
        if (request.ToDate.HasValue) ticketsQuery = ticketsQuery.Where(t => t.CreatedAt <= request.ToDate.Value);
        if (request.CinemaId.HasValue) ticketsQuery = ticketsQuery.Where(t => t.Showtime.Room.CinemaId == request.CinemaId.Value);
        if (request.MovieId.HasValue) ticketsQuery = ticketsQuery.Where(t => t.Showtime.MovieId == request.MovieId.Value);

        var groupedQuery = ticketsQuery
            .GroupBy(t => new { t.Showtime.Movie.Id, t.Showtime.Movie.Title })
            .Select(g => new MovieRevenueDto
            {
                MovieId = g.Key.Id,
                MovieTitle = g.Key.Title,
                TicketsSold = g.Count(),
                Revenue = g.Sum(t => t.Price)
            })
            .OrderByDescending(x => x.Revenue)
            .Take(request.TopN);

        return await groupedQuery.ToListAsync(cancellationToken);
    }
}
