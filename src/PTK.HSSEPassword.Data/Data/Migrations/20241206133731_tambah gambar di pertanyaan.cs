using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTK.HSSEPassport.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class tambahgambardipertanyaan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_FileUploads_ImageId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_ImageId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Answer");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Question",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_ImageId",
                table: "Question",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_FileUploads_ImageId",
                table: "Question",
                column: "ImageId",
                principalTable: "FileUploads",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_FileUploads_ImageId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_ImageId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Answer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_ImageId",
                table: "Answer",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_FileUploads_ImageId",
                table: "Answer",
                column: "ImageId",
                principalTable: "FileUploads",
                principalColumn: "Id");
        }
    }
}
