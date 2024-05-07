using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CarBookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Drivers_OwnerId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleDetails_Drivers_Id",
                table: "VehicleDetails");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRide",
                table: "UserRide");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserRide",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRide",
                table: "UserRide",
                column: "Id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Driver_Years_Of_EXP_PositiveNr",
                table: "Users",
                sql: " \"YearsOfExperience\" >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserRide_PassengerId",
                table: "UserRide",
                column: "PassengerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Users_OwnerId",
                table: "Rides",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleDetails_Users_Id",
                table: "VehicleDetails",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Users_OwnerId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleDetails_Users_Id",
                table: "VehicleDetails");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Driver_Years_Of_EXP_PositiveNr",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRide",
                table: "UserRide");

            migrationBuilder.DropIndex(
                name: "IX_UserRide_PassengerId",
                table: "UserRide");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserRide");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRide",
                table: "UserRide",
                columns: new[] { "PassengerId", "RideId" });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.CheckConstraint("CK_Driver_Years_Of_EXP_PositiveNr", " \"YearsOfExperience\" >= 0");
                    table.ForeignKey(
                        name: "FK_Drivers_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Drivers_OwnerId",
                table: "Rides",
                column: "OwnerId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleDetails_Drivers_Id",
                table: "VehicleDetails",
                column: "Id",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
