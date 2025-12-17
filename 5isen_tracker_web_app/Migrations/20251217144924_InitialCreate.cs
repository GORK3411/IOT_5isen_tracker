using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace _5isen_tracker_web_app.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "water_containers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    QrCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    HeightCm = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    Shape = table.Column<int>(type: "integer", nullable: false),
                    RadiusCm = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    LengthCm = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    WidthCm = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    MaxLiters = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_water_containers", x => x.Id);
                    table.CheckConstraint("ck_wc_dims_by_shape", "\r\n                    (\r\n                        \"Shape\" = 1 AND \"RadiusCm\" IS NOT NULL\r\n                        AND \"LengthCm\" IS NULL AND \"WidthCm\" IS NULL AND \"MaxLiters\" IS NULL\r\n                    )\r\n                    OR\r\n                    (\r\n                        \"Shape\" = 2 AND \"LengthCm\" IS NOT NULL AND \"WidthCm\" IS NOT NULL\r\n                        AND \"RadiusCm\" IS NULL AND \"MaxLiters\" IS NULL\r\n                    )\r\n                    OR\r\n                    (\r\n                        \"Shape\" = 3 AND \"MaxLiters\" IS NOT NULL\r\n                        AND \"RadiusCm\" IS NULL AND \"LengthCm\" IS NULL AND \"WidthCm\" IS NULL\r\n                    )\r\n                    ");
                    table.CheckConstraint("ck_wc_height_positive", "\"HeightCm\" > 0");
                    table.ForeignKey(
                        name: "FK_water_containers_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WaterContainerId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DistanceCm = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    WaterHeightCm = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    WaterPercent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    WaterLiters = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_logs_water_containers_WaterContainerId",
                        column: x => x.WaterContainerId,
                        principalTable: "water_containers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_logs_WaterContainerId_CreatedAt",
                table: "logs",
                columns: new[] { "WaterContainerId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_water_containers_QrCode",
                table: "water_containers",
                column: "QrCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_water_containers_UserId",
                table: "water_containers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "water_containers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
