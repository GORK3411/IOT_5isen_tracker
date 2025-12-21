using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _5isen_tracker_dll.Migrations
{
    /// <inheritdoc />
    public partial class afterUIMerge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_water_containers_DeviceId",
                table: "water_containers");

            migrationBuilder.AlterColumn<string>(
                name: "NodeId",
                table: "Devices",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_water_containers_DeviceId",
                table: "water_containers",
                column: "DeviceId",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_device_nodeid_len",
                table: "Devices",
                sql: "length(\"NodeId\") = 16");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_water_containers_DeviceId",
                table: "water_containers");

            migrationBuilder.DropCheckConstraint(
                name: "ck_device_nodeid_len",
                table: "Devices");

            migrationBuilder.AlterColumn<string>(
                name: "NodeId",
                table: "Devices",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16);

            migrationBuilder.CreateIndex(
                name: "IX_water_containers_DeviceId",
                table: "water_containers",
                column: "DeviceId");
        }
    }
}
