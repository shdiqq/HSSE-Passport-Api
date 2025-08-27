using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTK.HSSEPassport.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class tabelelementrelasiquestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElementId",
                table: "Question",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_ElementId",
                table: "Question",
                column: "ElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Element_ElementId",
                table: "Question",
                column: "ElementId",
                principalTable: "Element",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Element_ElementId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_ElementId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ElementId",
                table: "Question");
        }
    }
}
