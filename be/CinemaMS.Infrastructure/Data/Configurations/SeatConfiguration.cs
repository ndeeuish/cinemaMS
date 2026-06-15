using CinemaMS.Domain.Entities.Cinemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaMS.Infrastructure.Data.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        var seats = new List<Seat>();
        int id = 1001;
        for (int roomId = 101; roomId <= 104; roomId++)
        {
            for (int r = 0; r < 5; r++) // rows A-E
            {
                for (int c = 0; c < 6; c++) // 6 seats per row
                {
                    seats.Add(new Seat 
                    { 
                        Id = id++, RoomId = roomId, RowIndex = ((char)('A' + r)).ToString(), 
                        ColumnIndex = c, SeatNumber = c + 1, SeatTypeId = (r >= 3) ? 2 : 1, // VIP for row D, E
                        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false
                    });
                }
            }
        }
        builder.HasData(seats);
    }
}
