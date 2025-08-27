using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTK.HSSEPassport.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class tambahtambelelementuntuksoaldangambar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Answer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Element",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Element", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_FileUploads_ImageId",
                table: "Answer");

            migrationBuilder.DropTable(
                name: "Element");

            migrationBuilder.DropIndex(
                name: "IX_Answer_ImageId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Answer");
        }
    }
}
