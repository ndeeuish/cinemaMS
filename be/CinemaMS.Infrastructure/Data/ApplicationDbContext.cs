using CinemaMS.Application.Interfaces.Data;
using CinemaMS.Application.Interfaces.Security;
using System.Linq.Expressions;
using CinemaMS.Domain.Common;
using CinemaMS.Domain.Entities.Catalog;
using CinemaMS.Domain.Entities.Cinemas;
using CinemaMS.Domain.Entities.Identity;
using CinemaMS.Domain.Entities.Bookings;
using CinemaMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
{
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<AgeRestriction> AgeRestrictions { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<SeatType> SeatTypes { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Showtime> Showtimes { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Payment> Payments { get; set; }

    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var isDeletedProperty = entityType.FindProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.ClrType == typeof(bool))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var filter = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, isDeletedProperty.PropertyInfo!),
                        Expression.Constant(false, typeof(bool))
                    ),
                    parameter
                );
                entityType.SetQueryFilter(filter);
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService?.UserId;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                var createdAtProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "CreatedAt");
                if (createdAtProperty != null) createdAtProperty.CurrentValue = DateTime.UtcNow;

                var createdByProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "CreatedBy");
                if (createdByProperty != null && userId != null) createdByProperty.CurrentValue = userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                var updatedAtProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "UpdatedAt");
                if (updatedAtProperty != null) updatedAtProperty.CurrentValue = DateTime.UtcNow;

                var updatedByProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "UpdatedBy");
                if (updatedByProperty != null && userId != null) updatedByProperty.CurrentValue = userId;
            }
            else if (entry.State == EntityState.Deleted)
            {
                var isDeletedProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "IsDeleted");
                if (isDeletedProperty != null)
                {
                    entry.State = EntityState.Modified;
                    isDeletedProperty.CurrentValue = true;

                    var deletedDateProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "DeletedDate");
                    if (deletedDateProperty != null) deletedDateProperty.CurrentValue = DateTime.UtcNow;

                    var deletedByProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "DeletedBy");
                    if (deletedByProperty != null && userId != null) deletedByProperty.CurrentValue = userId;
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
