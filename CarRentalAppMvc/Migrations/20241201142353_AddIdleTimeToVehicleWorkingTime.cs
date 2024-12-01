using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAppMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddIdleTimeToVehicleWorkingTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "IdleTime",
                table: "vehicleWorkingTimes",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdleTime",
                table: "vehicleWorkingTimes");
        }
    }
}
