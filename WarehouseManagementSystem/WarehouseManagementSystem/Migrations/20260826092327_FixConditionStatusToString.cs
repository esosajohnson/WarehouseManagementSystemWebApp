using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixConditionStatusToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnTransaction_Location_LocationId",
                table: "ReturnTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnTransaction_Shipment_ShipmentId",
                table: "ReturnTransaction");

            migrationBuilder.AlterColumn<string>(
                name: "ConditionStatus",
                table: "ReturnTransaction",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnTransaction_Location_LocationId",
                table: "ReturnTransaction",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnTransaction_Shipment_ShipmentId",
                table: "ReturnTransaction",
                column: "ShipmentId",
                principalTable: "Shipment",
                principalColumn: "ShipmentId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnTransaction_Location_LocationId",
                table: "ReturnTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnTransaction_Shipment_ShipmentId",
                table: "ReturnTransaction");

            migrationBuilder.AlterColumn<int>(
                name: "ConditionStatus",
                table: "ReturnTransaction",
                type: "int",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Pending");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnTransaction_Location_LocationId",
                table: "ReturnTransaction",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnTransaction_Shipment_ShipmentId",
                table: "ReturnTransaction",
                column: "ShipmentId",
                principalTable: "Shipment",
                principalColumn: "ShipmentId");
        }
    }
}
