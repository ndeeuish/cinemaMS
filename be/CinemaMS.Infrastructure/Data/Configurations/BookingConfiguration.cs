using CinemaMS.Domain.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaMS.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        // Prevent SQL Server multiple cascade path errors
        builder.HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Booking { Id = 101, UserId = 101, TotalAmount = 200000, Status = "Confirmed", HoldExpiration = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Booking { Id = 102, UserId = 101, TotalAmount = 180000, Status = "Confirmed", HoldExpiration = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Booking { Id = 103, UserId = 101, TotalAmount = 170000, Status = "Confirmed", HoldExpiration = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Booking { Id = 104, UserId = 101, TotalAmount = 240000, Status = "Confirmed", HoldExpiration = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Booking { Id = 105, UserId = 101, TotalAmount = 190000, Status = "Confirmed", HoldExpiration = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false }
        );
    }
}
