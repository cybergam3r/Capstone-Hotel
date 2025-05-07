using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddDisponibileToServiziExtra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disponibile",
                table: "ServiziExtra",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Prenotazioni",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "PrenotazioniServizi",
                columns: table => new
                {
                    PrenotazioneId = table.Column<int>(type: "int", nullable: false),
                    ServizioExtraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrenotazioniServizi", x => new { x.PrenotazioneId, x.ServizioExtraId });
                    table.ForeignKey(
                        name: "FK_PrenotazioniServizi_Prenotazioni_PrenotazioneId",
                        column: x => x.PrenotazioneId,
                        principalTable: "Prenotazioni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrenotazioniServizi_ServiziExtra_ServizioExtraId",
                        column: x => x.ServizioExtraId,
                        principalTable: "ServiziExtra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prenotazioni_CameraId",
                table: "Prenotazioni",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_Prenotazioni_UserId",
                table: "Prenotazioni",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrenotazioniServizi_ServizioExtraId",
                table: "PrenotazioniServizi",
                column: "ServizioExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prenotazioni_AspNetUsers_UserId",
                table: "Prenotazioni",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prenotazioni_Camere_CameraId",
                table: "Prenotazioni",
                column: "CameraId",
                principalTable: "Camere",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prenotazioni_AspNetUsers_UserId",
                table: "Prenotazioni");

            migrationBuilder.DropForeignKey(
                name: "FK_Prenotazioni_Camere_CameraId",
                table: "Prenotazioni");

            migrationBuilder.DropTable(
                name: "PrenotazioniServizi");

            migrationBuilder.DropIndex(
                name: "IX_Prenotazioni_CameraId",
                table: "Prenotazioni");

            migrationBuilder.DropIndex(
                name: "IX_Prenotazioni_UserId",
                table: "Prenotazioni");

            migrationBuilder.DropColumn(
                name: "Disponibile",
                table: "ServiziExtra");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Prenotazioni",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
