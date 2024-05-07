using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarBookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovednavigationpropertyfromRideDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleDetail_Drivers_Id",
                table: "VehicleDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleDetail_Vehicles_VehicleId",
                table: "VehicleDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleDetail",
                table: "VehicleDetail");

            migrationBuilder.RenameTable(
                name: "VehicleDetail",
                newName: "VehicleDetails");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleDetail_VehicleId",
                table: "VehicleDetails",
                newName: "IX_VehicleDetails_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleDetail_RegistrationNumber",
                table: "VehicleDetails",
                newName: "IX_VehicleDetails_RegistrationNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleDetails",
                table: "VehicleDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleDetails_Drivers_Id",
                table: "VehicleDetails",
                column: "Id",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleDetails_Vehicles_VehicleId",
                table: "VehicleDetails",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleDetails_Drivers_Id",
                table: "VehicleDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleDetails_Vehicles_VehicleId",
                table: "VehicleDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleDetails",
                table: "VehicleDetails");

            migrationBuilder.RenameTable(
                name: "VehicleDetails",
                newName: "VehicleDetail");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleDetails_VehicleId",
                table: "VehicleDetail",
                newName: "IX_VehicleDetail_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleDetails_RegistrationNumber",
                table: "VehicleDetail",
                newName: "IX_VehicleDetail_RegistrationNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleDetail",
                table: "VehicleDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleDetail_Drivers_Id",
                table: "VehicleDetail",
                column: "Id",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleDetail_Vehicles_VehicleId",
                table: "VehicleDetail",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
