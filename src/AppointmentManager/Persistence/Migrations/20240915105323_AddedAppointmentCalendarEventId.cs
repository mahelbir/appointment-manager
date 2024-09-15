using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedAppointmentCalendarEventId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CalendarEventId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CalendarEventId",
                table: "Appointments",
                column: "CalendarEventId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_CalendarEventId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CalendarEventId",
                table: "Appointments");
        }
    }
}
