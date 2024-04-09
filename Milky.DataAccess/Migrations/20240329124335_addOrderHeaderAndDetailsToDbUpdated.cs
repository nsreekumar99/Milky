using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addOrderHeaderAndDetailsToDbUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderHeaders_HeaderOrderId",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "HeaderOrderId",
                table: "OrderDetails",
                newName: "OrderHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_HeaderOrderId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_OrderHeaderId");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "OrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "OrderHeaderId",
                table: "OrderDetails",
                newName: "HeaderOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_OrderHeaderId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_HeaderOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderHeaders_HeaderOrderId",
                table: "OrderDetails",
                column: "HeaderOrderId",
                principalTable: "OrderHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
