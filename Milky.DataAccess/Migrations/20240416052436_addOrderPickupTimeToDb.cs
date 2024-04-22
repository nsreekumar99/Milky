using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addOrderPickupTimeToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "OrderPickupTime",
                table: "OrderHeaders",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderPickupTime",
                table: "OrderHeaders");
        }
    }
}
