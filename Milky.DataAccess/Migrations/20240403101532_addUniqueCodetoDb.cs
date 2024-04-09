using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addUniqueCodetoDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueCode",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueCode",
                table: "OrderHeaders");
        }
    }
}
