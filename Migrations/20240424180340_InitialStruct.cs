using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cia_aerea_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialStruct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_airplanes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Manufacturer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_airplanes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_pilots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Registration = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_pilots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_maintenances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintenanceDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TypeOfMaintenance = table.Column<int>(type: "int", nullable: false),
                    AirplaneId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_maintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_maintenances_tb_airplanes_AirplaneId",
                        column: x => x.AirplaneId,
                        principalTable: "tb_airplanes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origin = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Destiny = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DepartureDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AirplaneId = table.Column<int>(type: "int", nullable: false),
                    PilotId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_flights_tb_airplanes_AirplaneId",
                        column: x => x.AirplaneId,
                        principalTable: "tb_airplanes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_flights_tb_pilots_PilotId",
                        column: x => x.PilotId,
                        principalTable: "tb_pilots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_flight_cancelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CancelationReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NotificationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_flight_cancelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_flight_cancelations_tb_flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "tb_flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_flight_cancelations_FlightId",
                table: "tb_flight_cancelations",
                column: "FlightId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_flights_AirplaneId",
                table: "tb_flights",
                column: "AirplaneId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_flights_PilotId",
                table: "tb_flights",
                column: "PilotId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_maintenances_AirplaneId",
                table: "tb_maintenances",
                column: "AirplaneId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pilots_Registration",
                table: "tb_pilots",
                column: "Registration",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_flight_cancelations");

            migrationBuilder.DropTable(
                name: "tb_maintenances");

            migrationBuilder.DropTable(
                name: "tb_flights");

            migrationBuilder.DropTable(
                name: "tb_airplanes");

            migrationBuilder.DropTable(
                name: "tb_pilots");
        }
    }
}
