using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarBookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "EntitySequence");

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"EntitySequence\"')"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"EntitySequence\"')"),
                    FacilityType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"EntitySequence\"')"),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    age = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("CK_User_Age_Adult", "age >= 18");
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"EntitySequence\"')"),
                    Make = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RideReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"EntitySequence\"')"),
                    ReviewerId = table.Column<int>(type: "integer", nullable: false),
                    RevieweeId = table.Column<int>(type: "integer", nullable: false),
                    rating = table.Column<float>(type: "real", nullable: false),
                    ReviewComment = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideReviews", x => x.Id);
                    table.CheckConstraint("CK_RideReviews_Rating", "rating >= 0");
                    table.ForeignKey(
                        name: "FK_RideReviews_Users_RevieweeId",
                        column: x => x.RevieweeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideReviews_Users_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"EntitySequence\"')"),
                    DateOfTheRide = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DestinationFromId = table.Column<int>(type: "integer", nullable: false),
                    DestinationToId = table.Column<int>(type: "integer", nullable: false),
                    available_seats = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.Id);
                    table.CheckConstraint("CK_Ride_AvailableSeats", "available_seats >= 1 AND available_seats <= 6");
                    table.ForeignKey(
                        name: "FK_Rides_Destinations_DestinationFromId",
                        column: x => x.DestinationFromId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rides_Destinations_DestinationToId",
                        column: x => x.DestinationToId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rides_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVehicle",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    VehiclesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVehicle", x => new { x.UserId, x.VehiclesId });
                    table.ForeignKey(
                        name: "FK_UserVehicle_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVehicle_Vehicles_VehiclesId",
                        column: x => x.VehiclesId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassengerRide",
                columns: table => new
                {
                    PassengerId = table.Column<int>(type: "integer", nullable: false),
                    RideId = table.Column<int>(type: "integer", nullable: false),
                    BookingStatus = table.Column<string>(type: "text", nullable: false, defaultValue: "PENDING"),
                    RideStatus = table.Column<string>(type: "text", nullable: false, defaultValue: "UPCOMING"),
                    PassengersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerRide", x => new { x.PassengerId, x.RideId });
                    table.ForeignKey(
                        name: "FK_PassengerRide_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerRide_Users_PassengersId",
                        column: x => x.PassengersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RideDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"EntitySequence\"')"),
                    RideId = table.Column<int>(type: "integer", nullable: false),
                    PickUpSpot = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideDetails", x => x.Id);
                    table.CheckConstraint("CK_RideDetail_Price_GreaterThanZero", "price >= 0");
                    table.ForeignKey(
                        name: "FK_RideDetails_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacilityRideDetail",
                columns: table => new
                {
                    FacilitiesId = table.Column<int>(type: "integer", nullable: false),
                    RideDetailId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityRideDetail", x => new { x.FacilitiesId, x.RideDetailId });
                    table.ForeignKey(
                        name: "FK_FacilityRideDetail_Facilities_FacilitiesId",
                        column: x => x.FacilitiesId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityRideDetail_RideDetails_RideDetailId",
                        column: x => x.RideDetailId,
                        principalTable: "RideDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilityRideDetail_RideDetailId",
                table: "FacilityRideDetail",
                column: "RideDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerRide_PassengersId",
                table: "PassengerRide",
                column: "PassengersId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerRide_RideId",
                table: "PassengerRide",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_RideDetails_RideId",
                table: "RideDetails",
                column: "RideId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RideReviews_RevieweeId",
                table: "RideReviews",
                column: "RevieweeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RideReviews_ReviewerId",
                table: "RideReviews",
                column: "ReviewerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DestinationFromId",
                table: "Rides",
                column: "DestinationFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DestinationToId",
                table: "Rides",
                column: "DestinationToId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_OwnerId",
                table: "Rides",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserVehicle_VehiclesId",
                table: "UserVehicle",
                column: "VehiclesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacilityRideDetail");

            migrationBuilder.DropTable(
                name: "PassengerRide");

            migrationBuilder.DropTable(
                name: "RideReviews");

            migrationBuilder.DropTable(
                name: "UserVehicle");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "RideDetails");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Rides");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropSequence(
                name: "EntitySequence");
        }
    }
}
