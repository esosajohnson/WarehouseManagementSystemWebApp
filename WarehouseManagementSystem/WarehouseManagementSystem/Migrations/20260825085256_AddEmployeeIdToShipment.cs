using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeIdToShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Shipment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_EmployeeId",
                table: "Shipment",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_Employee_EmployeeId",
                table: "Shipment",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_Employee_EmployeeId",
                table: "Shipment");

            migrationBuilder.DropIndex(
                name: "IX_Shipment_EmployeeId",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Shipment");
        }
    }
}
