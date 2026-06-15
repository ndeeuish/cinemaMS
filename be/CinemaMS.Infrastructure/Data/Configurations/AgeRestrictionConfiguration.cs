using CinemaMS.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaMS.Infrastructure.Data.Configurations;

public class AgeRestrictionConfiguration : IEntityTypeConfiguration<AgeRestriction>
{
    public void Configure(EntityTypeBuilder<AgeRestriction> builder)
    {
        builder.HasData(
            new AgeRestriction { Id = 1, Code = "P", Description = "Phổ biến, dành cho mọi lứa tuổi", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new AgeRestriction { Id = 2, Code = "T13", Description = "Phim phổ biến đến người xem từ 13 tuổi trở lên", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new AgeRestriction { Id = 3, Code = "T16", Description = "Phim phổ biến đến người xem từ 16 tuổi trở lên", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false },
            new AgeRestriction { Id = 4, Code = "T18", Description = "Phim phổ biến đến người xem từ 18 tuổi trở lên", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), IsDeleted = false }
        );
    }
}
