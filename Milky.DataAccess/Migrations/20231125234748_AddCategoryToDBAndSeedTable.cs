using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToDBAndSeedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Category",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Milk");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "Curd");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "Ghee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Action");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "SciFi");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "History");
        }
    }
}
