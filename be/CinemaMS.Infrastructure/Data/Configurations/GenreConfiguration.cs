using CinemaMS.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaMS.Infrastructure.Data.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasData(
            new Genre { Id = 1, Name = "Hành động", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Genre { Id = 2, Name = "Hài hước", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Genre { Id = 3, Name = "Kinh dị", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Genre { Id = 4, Name = "Tình cảm", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Genre { Id = 5, Name = "Hoạt hình", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Genre { Id = 6, Name = "Viễn tưởng", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false }
        );
    }
}
