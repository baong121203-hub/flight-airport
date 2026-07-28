using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IataCode = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, defaultValue: "Vietnam")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlightNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FlightDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ArrDep = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    OriginAirportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationAirportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AircraftReg = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AircraftType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FlightType = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    NatureOfFlight = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "---"),
                    ScheduledTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParkingStand = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Gate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BookingPax = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.CheckConstraint("CK_Flights_ArrDep", "[ArrDep] IN ('A', 'D')");
                    table.CheckConstraint("CK_Flights_FlightType", "[FlightType] IN ('PAX', 'CGO')");
                    table.CheckConstraint("CK_Flights_OriginDest", "[OriginAirportId] <> [DestinationAirportId]");
                    table.CheckConstraint("CK_Flights_Status", "[Status] IN ('OPN', 'DLY', 'CNX', 'CLS', 'XXX')");
                    table.ForeignKey(
                        name: "FK_Flights_Airports_DestinationAirportId",
                        column: x => x.DestinationAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_Airports_OriginAirportId",
                        column: x => x.OriginAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airports_IataCode",
                table: "Airports",
                column: "IataCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationAirportId",
                table: "Flights",
                column: "DestinationAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightDate",
                table: "Flights",
                column: "FlightDate");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightNo_FlightDate_OriginAirportId_DestinationAirportId_ArrDep",
                table: "Flights",
                columns: new[] { "FlightNo", "FlightDate", "OriginAirportId", "DestinationAirportId", "ArrDep" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_OriginAirportId",
                table: "Flights",
                column: "OriginAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_Status",
                table: "Flights",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Airports");
        }
    }
}
