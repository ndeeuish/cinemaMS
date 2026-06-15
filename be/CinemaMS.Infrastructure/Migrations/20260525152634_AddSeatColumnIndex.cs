using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatColumnIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColumnIndex",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 901,
                column: "ColumnIndex",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 902,
                column: "ColumnIndex",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 903,
                column: "ColumnIndex",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 904,
                column: "ColumnIndex",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 905,
                column: "ColumnIndex",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 906,
                column: "ColumnIndex",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnIndex",
                table: "Seats");
        }
    }
}
