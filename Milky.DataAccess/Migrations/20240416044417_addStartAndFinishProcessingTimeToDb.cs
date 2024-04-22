using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Milky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addStartAndFinishProcessingTimeToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "FinishedProcessingTime",
                table: "OrderHeaders",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartedProcessingTime",
                table: "OrderHeaders",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedProcessingTime",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "StartedProcessingTime",
                table: "OrderHeaders");
        }
    }
}
