using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_ShowtimeId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ShowtimeId_SeatId",
                table: "Tickets",
                columns: new[] { "ShowtimeId", "SeatId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_ShowtimeId_SeatId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ShowtimeId",
                table: "Tickets",
                column: "ShowtimeId");
        }
    }
}
