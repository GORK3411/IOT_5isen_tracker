using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace _5isen_tracker_dll.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_logs_water_containers_WaterContainerId",
                table: "logs");

            migrationBuilder.DropForeignKey(
                name: "FK_water_containers_users_UserId",
                table: "water_containers");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_logs",
                table: "logs");

            migrationBuilder.DropIndex(
                name: "IX_logs_WaterContainerId_CreatedAt",
                table: "logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_water_containers",
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
                name: "WaterHeightCm",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "WaterLiters",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "WaterPercent",
                table: "logs");

            migrationBuilder.RenameTable(
                name: "logs",
                newName: "Logs");

            migrationBuilder.RenameTable(
                name: "water_containers",
                newName: "WaterContainers");

            migrationBuilder.RenameColumn(
                name: "WaterContainerId",
                table: "Logs",
                newName: "DeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_water_containers_UserId",
                table: "WaterContainers",
                newName: "IX_WaterContainers_UserId");

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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WaterContainers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaterContainers",
                table: "WaterContainers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NodeId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_DeviceId",
                table: "Logs",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Devices_DeviceId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterContainers_AspNetUsers_UserId",
                table: "WaterContainers");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_DeviceId",
                table: "Logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaterContainers",
                table: "WaterContainers");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "logs");

            migrationBuilder.RenameTable(
                name: "WaterContainers",
                newName: "water_containers");

            migrationBuilder.RenameColumn(
                name: "DeviceId",
                table: "logs",
                newName: "WaterContainerId");

            migrationBuilder.RenameIndex(
                name: "IX_WaterContainers_UserId",
                table: "water_containers",
                newName: "IX_water_containers_UserId");

            migrationBuilder.AlterColumn<decimal>(
                name: "DistanceCm",
                table: "logs",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<decimal>(
                name: "WaterHeightCm",
                table: "logs",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WaterLiters",
                table: "logs",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WaterPercent",
                table: "logs",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "water_containers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_logs",
                table: "logs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_water_containers",
                table: "water_containers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_logs_WaterContainerId_CreatedAt",
                table: "logs",
                columns: new[] { "WaterContainerId", "CreatedAt" });

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

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_logs_water_containers_WaterContainerId",
                table: "logs",
                column: "WaterContainerId",
                principalTable: "water_containers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_water_containers_users_UserId",
                table: "water_containers",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
