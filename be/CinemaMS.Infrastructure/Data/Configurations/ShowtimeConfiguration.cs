using CinemaMS.Domain.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaMS.Infrastructure.Data.Configurations;

public class ShowtimeConfiguration : IEntityTypeConfiguration<Showtime>
{
    public void Configure(EntityTypeBuilder<Showtime> builder)
    {
        builder.HasData(
            new Showtime { Id = 101, MovieId = 101, RoomId = 101, StartTime = new DateTime(2026, 6, 1, 18, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 6, 1, 21, 0, 0, DateTimeKind.Utc), BasePrice = 100000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Showtime { Id = 102, MovieId = 102, RoomId = 102, StartTime = new DateTime(2026, 6, 1, 19, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 6, 1, 21, 30, 0, DateTimeKind.Utc), BasePrice = 90000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Showtime { Id = 103, MovieId = 103, RoomId = 103, StartTime = new DateTime(2026, 6, 1, 20, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 6, 1, 22, 30, 0, DateTimeKind.Utc), BasePrice = 85000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Showtime { Id = 104, MovieId = 104, RoomId = 104, StartTime = new DateTime(2026, 6, 1, 17, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 6, 1, 20, 0, 0, DateTimeKind.Utc), BasePrice = 120000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Showtime { Id = 105, MovieId = 105, RoomId = 101, StartTime = new DateTime(2026, 6, 1, 21, 30, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 6, 1, 23, 30, 0, DateTimeKind.Utc), BasePrice = 95000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false }
        );
    }
}
