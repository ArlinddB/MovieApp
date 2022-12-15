using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Redeo.Migrations
{
    /// <inheritdoc />
    public partial class ProducersFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDay",
                table: "producers");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "producers",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "producers");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDay",
                table: "producers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
