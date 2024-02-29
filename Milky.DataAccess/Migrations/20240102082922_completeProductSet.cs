using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class completeProductSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BiologicalSource",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DietType",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Flavour",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemForm",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NetQuantity",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NumberOfItems",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "BiologicalSource", "DietType", "Flavour", "ItemForm", "NetQuantity", "NumberOfItems" },
                values: new object[] { null, null, null, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiologicalSource",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DietType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Flavour",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ItemForm",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NetQuantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NumberOfItems",
                table: "Products");
        }
    }
}
