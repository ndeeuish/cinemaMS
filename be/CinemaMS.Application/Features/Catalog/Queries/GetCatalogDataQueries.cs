using CinemaMS.Application.Features.Catalog.DTOs;
using CinemaMS.Application.Interfaces.Data;
using CinemaMS.Application.Messaging;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Application.Features.Catalog.Queries;

// 1. Genres
public class GetAllGenresQuery : QueryBase<List<GenreDto>> { }

public class GetAllGenresQueryHandler : QueryHandlerBase<GetAllGenresQuery, List<GenreDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllGenresQueryHandler(IApplicationDbContext context) => _context = context;

    public override async Task<List<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        return await _context.Genres
            .Select(g => new GenreDto { Id = g.Id, Name = g.Name })
            .ToListAsync(cancellationToken);
    }
}

// 2. Age Restrictions
public class GetAllAgeRestrictionsQuery : QueryBase<List<AgeRestrictionDto>> { }

public class GetAllAgeRestrictionsQueryHandler : QueryHandlerBase<GetAllAgeRestrictionsQuery, List<AgeRestrictionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllAgeRestrictionsQueryHandler(IApplicationDbContext context) => _context = context;

    public override async Task<List<AgeRestrictionDto>> Handle(GetAllAgeRestrictionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.AgeRestrictions
            .Select(a => new AgeRestrictionDto { Id = a.Id, Code = a.Code, Description = a.Description })
            .ToListAsync(cancellationToken);
    }
}

// 3. Room Types
public class GetAllRoomTypesQuery : QueryBase<List<RoomTypeDto>> { }

public class GetAllRoomTypesQueryHandler : QueryHandlerBase<GetAllRoomTypesQuery, List<RoomTypeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllRoomTypesQueryHandler(IApplicationDbContext context) => _context = context;

    public override async Task<List<RoomTypeDto>> Handle(GetAllRoomTypesQuery request, CancellationToken cancellationToken)
    {
        return await _context.RoomTypes
            .Select(r => new RoomTypeDto { Id = r.Id, Name = r.Name, Surcharge = r.Surcharge })
            .ToListAsync(cancellationToken);
    }
}

// 4. Seat Types
public class GetAllSeatTypesQuery : QueryBase<List<SeatTypeDto>> { }

public class GetAllSeatTypesQueryHandler : QueryHandlerBase<GetAllSeatTypesQuery, List<SeatTypeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllSeatTypesQueryHandler(IApplicationDbContext context) => _context = context;

    public override async Task<List<SeatTypeDto>> Handle(GetAllSeatTypesQuery request, CancellationToken cancellationToken)
    {
        return await _context.SeatTypes
            .Select(s => new SeatTypeDto { Id = s.Id, Name = s.Name, Surcharge = s.Surcharge })
            .ToListAsync(cancellationToken);
    }
}
