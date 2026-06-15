using CinemaMS.Domain.Entities.Cinemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaMS.Infrastructure.Data.Configurations;

public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
{
    public void Configure(EntityTypeBuilder<Cinema> builder)
    {
        builder.HasData(
            new Cinema { Id = 101, Name = "CGV Vincom Landmark 81", Address = "720A Dien Bien Phu, Binh Thanh", Hotline = "19001111", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Cinema { Id = 102, Name = "Lotte Cinema Nam Sài Gòn", Address = "469 Nguyen Huu Tho, Dist 7", Hotline = "19002222", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false }
        );
    }
}
