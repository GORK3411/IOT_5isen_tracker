using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _5isen_tracker_dll.Migrations
{
    /// <inheritdoc />
    public partial class removedFkLogWaterContainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_logs_water_containers_WaterContainerId",
                table: "logs");

            migrationBuilder.DropIndex(
                name: "IX_logs_WaterContainerId",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "WaterContainerId",
                table: "logs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WaterContainerId",
                table: "logs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_logs_WaterContainerId",
                table: "logs",
                column: "WaterContainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_logs_water_containers_WaterContainerId",
                table: "logs",
                column: "WaterContainerId",
                principalTable: "water_containers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
