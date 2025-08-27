using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTK.HSSEPassport.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class addwrongpss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WrongPss",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WrongPss",
                table: "User");
        }
    }
}
