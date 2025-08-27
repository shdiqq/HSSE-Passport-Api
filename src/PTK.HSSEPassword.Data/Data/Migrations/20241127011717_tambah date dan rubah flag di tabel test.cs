using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTK.HSSEPassport.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class tambahdatedanrubahflagditabeltest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Test",
                newName: "Flag");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Test",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Passport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Passport");

            migrationBuilder.RenameColumn(
                name: "Flag",
                table: "Test",
                newName: "Type");
        }
    }
}
