using CinemaMS.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaMS.Infrastructure.Data.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasData(
            new Movie { Id = 101, Title = "Avatar: The Way of Water", Description = "Jake Sully lives with his newfound family formed on the extrasolar moon Pandora.", DurationInMinutes = 192, ReleaseDate = new DateTime(2022, 12, 16, 0, 0, 0, DateTimeKind.Utc), Director = "James Cameron", Casts = "Sam Worthington, Zoe Saldana", PosterUrl = "https://m.media-amazon.com/images/M/MV5BYjhiNjBlODctY2ZiOC00YjVlLWFlNzAtNTVhNzM1YjI1NzMxXkEyXkFqcGdeQXVyMjQxNTE1MDA@._V1_FMjpg_UX1000_.jpg", TrailerUrl = "https://youtube.com/avatar2", AgeRestrictionId = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Movie { Id = 102, Title = "Inception", Description = "A thief who steals corporate secrets through the use of dream-sharing technology.", DurationInMinutes = 148, ReleaseDate = new DateTime(2010, 7, 16, 0, 0, 0, DateTimeKind.Utc), Director = "Christopher Nolan", Casts = "Leonardo DiCaprio, Joseph Gordon-Levitt", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_FMjpg_UX1000_.jpg", TrailerUrl = "https://youtube.com/inception", AgeRestrictionId = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Movie { Id = 103, Title = "The Dark Knight", Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham.", DurationInMinutes = 152, ReleaseDate = new DateTime(2008, 7, 18, 0, 0, 0, DateTimeKind.Utc), Director = "Christopher Nolan", Casts = "Christian Bale, Heath Ledger", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_FMjpg_UX1000_.jpg", TrailerUrl = "https://youtube.com/tdk", AgeRestrictionId = 3, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Movie { Id = 104, Title = "Interstellar", Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.", DurationInMinutes = 169, ReleaseDate = new DateTime(2014, 11, 7, 0, 0, 0, DateTimeKind.Utc), Director = "Christopher Nolan", Casts = "Matthew McConaughey, Anne Hathaway", PosterUrl = "https://m.media-amazon.com/images/M/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_FMjpg_UX1000_.jpg", TrailerUrl = "https://youtube.com/interstellar", AgeRestrictionId = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new Movie { Id = 105, Title = "Spider-Man: No Way Home", Description = "With Spider-Man's identity now revealed, Peter asks Doctor Strange for help.", DurationInMinutes = 148, ReleaseDate = new DateTime(2021, 12, 17, 0, 0, 0, DateTimeKind.Utc), Director = "Jon Watts", Casts = "Tom Holland, Zendaya", PosterUrl = "https://m.media-amazon.com/images/M/MV5BZWMyYzFjYTYtNTRjZi00NjU2LWE4MTItLWFhMTFiZDA1MzYwXkEyXkFqcGdeQXVyNTA3MTU2MjI@._V1_FMjpg_UX1000_.jpg", TrailerUrl = "https://youtube.com/nwh", AgeRestrictionId = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false }
        );
    }
}
