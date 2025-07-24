using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionAndStockERP.Migrations
{
    /// <inheritdoc />
    public partial class activityLogsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Changes",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetEntity",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetEntityId",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Changes",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "TargetEntity",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "TargetEntityId",
                table: "ActivityLogs");
        }
    }
}
