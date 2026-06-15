using CinemaMS.Domain.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaMS.Infrastructure.Data.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        // Prevent race condition: A seat can only be booked once per showtime
        builder.HasIndex(t => new { t.ShowtimeId, t.SeatId })
            .IsUnique();

        // Prevent SQL Server multiple cascade path errors
        builder.HasOne(t => t.Showtime)
            .WithMany()
            .HasForeignKey(t => t.ShowtimeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Seat)
            .WithMany()
            .HasForeignKey(t => t.SeatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            // Booking 101 - Showtime 101 (Room 101) - Seats 1001, 1002
            new Ticket { Id = 1001, BookingId = 101, ShowtimeId = 101, SeatId = 1001, Price = 100000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Ticket { Id = 1002, BookingId = 101, ShowtimeId = 101, SeatId = 1002, Price = 100000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            
            // Booking 102 - Showtime 102 (Room 102) - Seats 1031, 1032
            new Ticket { Id = 1003, BookingId = 102, ShowtimeId = 102, SeatId = 1031, Price = 90000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Ticket { Id = 1004, BookingId = 102, ShowtimeId = 102, SeatId = 1032, Price = 90000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },

            // Booking 103 - Showtime 103 (Room 103) - Seats 1061, 1062
            new Ticket { Id = 1005, BookingId = 103, ShowtimeId = 103, SeatId = 1061, Price = 85000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Ticket { Id = 1006, BookingId = 103, ShowtimeId = 103, SeatId = 1062, Price = 85000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },

            // Booking 104 - Showtime 104 (Room 104) - Seats 1091, 1092
            new Ticket { Id = 1007, BookingId = 104, ShowtimeId = 104, SeatId = 1091, Price = 120000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Ticket { Id = 1008, BookingId = 104, ShowtimeId = 104, SeatId = 1092, Price = 120000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },

            // Booking 105 - Showtime 105 (Room 101) - Seats 1003, 1004
            new Ticket { Id = 1009, BookingId = 105, ShowtimeId = 105, SeatId = 1003, Price = 95000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Ticket { Id = 1010, BookingId = 105, ShowtimeId = 105, SeatId = 1004, Price = 95000, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false }
        );
    }
}
