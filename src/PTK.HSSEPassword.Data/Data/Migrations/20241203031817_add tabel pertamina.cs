using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTK.HSSEPassport.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class addtabelpertamina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FotoId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NIK",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PertaminaId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WrongPassword",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Pertamina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoId = table.Column<int>(type: "int", nullable: true),
                    OfficialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficialPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficialSignId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pertamina", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pertamina_FileUploads_LogoId",
                        column: x => x.LogoId,
                        principalTable: "FileUploads",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pertamina_FileUploads_OfficialSignId",
                        column: x => x.OfficialSignId,
                        principalTable: "FileUploads",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_FotoId",
                table: "User",
                column: "FotoId");

            migrationBuilder.CreateIndex(
                name: "IX_User_PertaminaId",
                table: "User",
                column: "PertaminaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pertamina_LogoId",
                table: "Pertamina",
                column: "LogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pertamina_OfficialSignId",
                table: "Pertamina",
                column: "OfficialSignId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_FileUploads_FotoId",
                table: "User",
                column: "FotoId",
                principalTable: "FileUploads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Pertamina_PertaminaId",
                table: "User",
                column: "PertaminaId",
                principalTable: "Pertamina",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_FileUploads_FotoId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Pertamina_PertaminaId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Pertamina");

            migrationBuilder.DropIndex(
                name: "IX_User_FotoId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PertaminaId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FotoId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NIK",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PertaminaId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WrongPassword",
                table: "User");
        }
    }
}
