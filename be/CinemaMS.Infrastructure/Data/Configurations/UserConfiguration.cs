using CinemaMS.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaMS.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User 
            { 
                Id = 101, 
                Username = "seeduser", 
                Email = "seeduser@example.com",
                FullName = "Seeded User",
                PhoneNumber = "0123456789",
                PasswordHash = "AQAAAAIAAYagAAAAENK+h5584/W3j4iP1sN5K07hC3hP1hP1hP1hP1hP1hP1hP1hP1hP1hP1hP1hP1hP", // Just a dummy hash
                RoleId = 2, // Customer
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), 
                IsDeleted = false 
            }
        );
    }
}
