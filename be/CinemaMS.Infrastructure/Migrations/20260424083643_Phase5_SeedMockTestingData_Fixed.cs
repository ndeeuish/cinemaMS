using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Phase5_SeedMockTestingData_Fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Address", "CreatedAt", "CreatedBy", "DeletedBy", "DeletedDate", "Hotline", "IsDeleted", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 99, "29 Lieu Giai, Ba Dinh, Ha Noi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "19001234", false, "CGV Vincom Metropolis (Mock)", null, null });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "AgeRestrictionId", "Casts", "CreatedAt", "CreatedBy", "DeletedBy", "DeletedDate", "Description", "Director", "DurationInMinutes", "IsDeleted", "PosterUrl", "ReleaseDate", "Title", "TrailerUrl", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 99, 2, "Robert Downey Jr., Pedro Pascal", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "The epic conclusion to the Multiverse Saga.", "Russo Brothers", 150, false, "https://example.com/avengers.jpg", new DateTime(2027, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Avengers: Secret Wars (Mock)", "https://youtube.com/avengers", null, null });

            migrationBuilder.InsertData(
                table: "MovieGenres",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedBy", "DeletedDate", "GenreId", "IsDeleted", "MovieId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 99, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 1, false, 99, null, null },
                    { 100, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 6, false, 99, null, null }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Capacity", "CinemaId", "CreatedAt", "CreatedBy", "DeletedBy", "DeletedDate", "IsDeleted", "Name", "RoomTypeId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 99, 6, 99, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, "Cinema 01 (IMAX - Mock)", 3, null, null });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedBy", "DeletedDate", "IsDeleted", "RoomId", "RowIndex", "SeatNumber", "SeatTypeId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 901, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 99, "A", 1, 1, null, null },
                    { 902, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 99, "A", 2, 1, null, null },
                    { 903, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 99, "A", 3, 1, null, null },
                    { 904, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 99, "B", 1, 2, null, null },
                    { 905, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 99, "B", 2, 2, null, null },
                    { 906, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 99, "B", 3, 3, null, null }
                });

            migrationBuilder.InsertData(
                table: "Showtimes",
                columns: new[] { "Id", "BasePrice", "CreatedAt", "CreatedBy", "DeletedBy", "DeletedDate", "EndTime", "IsDeleted", "MovieId", "RoomId", "StartTime", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 99, 80000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new DateTime(2026, 5, 1, 23, 0, 0, 0, DateTimeKind.Utc), false, 99, 99, new DateTime(2026, 5, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "MovieGenres",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 901);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 902);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 903);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 904);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 905);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 906);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 99);
        }
    }
}
