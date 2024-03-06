using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatePrimaryKeyForShoppingcart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shoppingCarts_Products_Productid",
                table: "shoppingCarts");

            migrationBuilder.RenameColumn(
                name: "Productid",
                table: "shoppingCarts",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "shoppingCarts",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_shoppingCarts_Productid",
                table: "shoppingCarts",
                newName: "IX_shoppingCarts_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingCarts_Products_ProductId",
                table: "shoppingCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shoppingCarts_Products_ProductId",
                table: "shoppingCarts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "shoppingCarts",
                newName: "Productid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "shoppingCarts",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_shoppingCarts_ProductId",
                table: "shoppingCarts",
                newName: "IX_shoppingCarts_Productid");

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingCarts_Products_Productid",
                table: "shoppingCarts",
                column: "Productid",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
