using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyForProductCategoryRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Category_CageoryID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CageoryID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CageoryID",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Category_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Category_CategoryID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryID",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "CageoryID",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "id",
                keyValue: 1,
                column: "CageoryID",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CageoryID",
                table: "Products",
                column: "CageoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Category_CageoryID",
                table: "Products",
                column: "CageoryID",
                principalTable: "Category",
                principalColumn: "id");
        }
    }
}
