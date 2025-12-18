using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _5isen_tracker_dll.Migrations
{
    /// <inheritdoc />
    public partial class seed_devices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Devices_DeviceId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterContainers_AspNetUsers_UserId",
                table: "WaterContainers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaterContainers",
                table: "WaterContainers");

            migrationBuilder.DropIndex(
                name: "IX_WaterContainers_UserId",
                table: "WaterContainers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WaterContainers");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "logs");

            migrationBuilder.RenameTable(
                name: "WaterContainers",
                newName: "water_containers");

            migrationBuilder.RenameIndex(
                name: "IX_Logs_DeviceId",
                table: "logs",
                newName: "IX_logs_DeviceId");

            migrationBuilder.AlterColumn<decimal>(
                name: "DistanceCm",
                table: "logs",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "WidthCm",
                table: "water_containers",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "RadiusCm",
                table: "water_containers",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLiters",
                table: "water_containers",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LengthCm",
                table: "water_containers",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "HeightCm",
                table: "water_containers",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "water_containers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_logs",
                table: "logs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_water_containers",
                table: "water_containers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_water_containers_DeviceId",
                table: "water_containers",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_water_containers_QrCode",
                table: "water_containers",
                column: "QrCode",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_wc_dims_by_shape",
                table: "water_containers",
                sql: "\r\n                    (\r\n                        \"Shape\" = 1 AND \"RadiusCm\" IS NOT NULL\r\n                        AND \"LengthCm\" IS NULL AND \"WidthCm\" IS NULL AND \"MaxLiters\" IS NULL\r\n                    )\r\n                    OR\r\n                    (\r\n                        \"Shape\" = 2 AND \"LengthCm\" IS NOT NULL AND \"WidthCm\" IS NOT NULL\r\n                        AND \"RadiusCm\" IS NULL AND \"MaxLiters\" IS NULL\r\n                    )\r\n                    OR\r\n                    (\r\n                        \"Shape\" = 3 AND \"MaxLiters\" IS NOT NULL\r\n                        AND \"RadiusCm\" IS NULL AND \"LengthCm\" IS NULL AND \"WidthCm\" IS NULL\r\n                    )\r\n                    ");

            migrationBuilder.AddCheckConstraint(
                name: "ck_wc_height_positive",
                table: "water_containers",
                sql: "\"HeightCm\" > 0");

            migrationBuilder.AddForeignKey(
                name: "FK_logs_Devices_DeviceId",
                table: "logs",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_water_containers_Devices_DeviceId",
                table: "water_containers",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_logs_Devices_DeviceId",
                table: "logs");

            migrationBuilder.DropForeignKey(
                name: "FK_water_containers_Devices_DeviceId",
                table: "water_containers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_logs",
                table: "logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_water_containers",
                table: "water_containers");

            migrationBuilder.DropIndex(
                name: "IX_water_containers_DeviceId",
                table: "water_containers");

            migrationBuilder.DropIndex(
                name: "IX_water_containers_QrCode",
                table: "water_containers");

            migrationBuilder.DropCheckConstraint(
                name: "ck_wc_dims_by_shape",
                table: "water_containers");

            migrationBuilder.DropCheckConstraint(
                name: "ck_wc_height_positive",
                table: "water_containers");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "water_containers");

            migrationBuilder.RenameTable(
                name: "logs",
                newName: "Logs");

            migrationBuilder.RenameTable(
                name: "water_containers",
                newName: "WaterContainers");

            migrationBuilder.RenameIndex(
                name: "IX_logs_DeviceId",
                table: "Logs",
                newName: "IX_Logs_DeviceId");

            migrationBuilder.AlterColumn<decimal>(
                name: "DistanceCm",
                table: "Logs",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "WidthCm",
                table: "WaterContainers",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "RadiusCm",
                table: "WaterContainers",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLiters",
                table: "WaterContainers",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LengthCm",
                table: "WaterContainers",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "HeightCm",
                table: "WaterContainers",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WaterContainers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaterContainers",
                table: "WaterContainers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WaterContainers_UserId",
                table: "WaterContainers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Devices_DeviceId",
                table: "Logs",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaterContainers_AspNetUsers_UserId",
                table: "WaterContainers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
