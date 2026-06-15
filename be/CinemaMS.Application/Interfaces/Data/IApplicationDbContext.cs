using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CinemaMS.Application.Interfaces.Data;

public interface IApplicationDbContext
{
    #region Identity
    DbSet<CinemaMS.Domain.Entities.Identity.User> Users { get; }
    DbSet<CinemaMS.Domain.Entities.Identity.Role> Roles { get; }
    DbSet<CinemaMS.Domain.Entities.Identity.Permission> Permissions { get; }
    DbSet<CinemaMS.Domain.Entities.Identity.RolePermission> RolePermissions { get; }

    #endregion

    #region Movie
    DbSet<CinemaMS.Domain.Entities.Cinemas.Cinema> Cinemas { get; }
    DbSet<CinemaMS.Domain.Entities.Catalog.Movie> Movies { get; }
    DbSet<CinemaMS.Domain.Entities.Catalog.AgeRestriction> AgeRestrictions { get; }
    DbSet<CinemaMS.Domain.Entities.Catalog.Genre> Genres { get; }
    DbSet<CinemaMS.Domain.Entities.Catalog.MovieGenre> MovieGenres { get; }

    #endregion

    #region Theater
    DbSet<CinemaMS.Domain.Entities.Cinemas.RoomType> RoomTypes { get; }
    DbSet<CinemaMS.Domain.Entities.Cinemas.Room> Rooms { get; }
    DbSet<CinemaMS.Domain.Entities.Cinemas.SeatType> SeatTypes { get; }
    DbSet<CinemaMS.Domain.Entities.Cinemas.Seat> Seats { get; }

    #endregion

    #region Booking
    DbSet<CinemaMS.Domain.Entities.Bookings.Showtime> Showtimes { get; }
    DbSet<CinemaMS.Domain.Entities.Bookings.Booking> Bookings { get; }
    DbSet<CinemaMS.Domain.Entities.Bookings.Ticket> Tickets { get; }
    DbSet<CinemaMS.Domain.Entities.Bookings.Payment> Payments { get; }

    #endregion


    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry Entry(object entity);
    ChangeTracker ChangeTracker { get; }
    DatabaseFacade Database { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
