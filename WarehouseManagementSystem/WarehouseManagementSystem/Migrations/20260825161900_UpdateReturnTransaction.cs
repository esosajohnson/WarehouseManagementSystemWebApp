using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReturnTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ConditionStatus",
                table: "ReturnTransaction",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "ReturnTransaction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShipmentId",
                table: "ReturnTransaction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnTransaction_LocationId",
                table: "ReturnTransaction",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnTransaction_ShipmentId",
                table: "ReturnTransaction",
                column: "ShipmentId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnTransaction_Location_LocationId",
                table: "ReturnTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnTransaction_Shipment_ShipmentId",
                table: "ReturnTransaction");

            migrationBuilder.DropIndex(
                name: "IX_ReturnTransaction_LocationId",
                table: "ReturnTransaction");

            migrationBuilder.DropIndex(
                name: "IX_ReturnTransaction_ShipmentId",
                table: "ReturnTransaction");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "ReturnTransaction");

            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "ReturnTransaction");

            migrationBuilder.AlterColumn<string>(
                name: "ConditionStatus",
                table: "ReturnTransaction",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50);
        }
    }
}
