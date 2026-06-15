using CinemaMS.Domain.Entities.Cinemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaMS.Infrastructure.Data.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasData(
            new Room { Id = 101, CinemaId = 101, Name = "Cinema 01 (IMAX)", Capacity = 30, RoomTypeId = 3, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Room { Id = 102, CinemaId = 101, Name = "Cinema 02 (Standard)", Capacity = 30, RoomTypeId = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Room { Id = 103, CinemaId = 102, Name = "Room 1 (Standard)", Capacity = 30, RoomTypeId = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Room { Id = 104, CinemaId = 102, Name = "Room 2 (3D)", Capacity = 30, RoomTypeId = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false }
        );
    }
}
