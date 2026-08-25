using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeToShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_Employee_EmployeeId",
                table: "Shipment");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_Employee_EmployeeId",
                table: "Shipment",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_Employee_EmployeeId",
                table: "Shipment");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_Employee_EmployeeId",
                table: "Shipment",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId");
        }
    }
}
