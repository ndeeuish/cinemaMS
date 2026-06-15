using CinemaMS.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaMS.Infrastructure.Data.Configurations;

public class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.HasData(
            new MovieGenre { Id = 99, MovieId = 99, GenreId = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false }, // Hành động
            new MovieGenre { Id = 100, MovieId = 99, GenreId = 6, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false }  // Viễn tưởng
        );
    }
}
