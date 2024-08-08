using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class MakeProprietarioIdAndTipologiaAnimaleNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clienti_Proprietari_ProprietarioId",
                table: "Clienti");

            migrationBuilder.AlterColumn<string>(
                name: "TipologiaAnimale",
                table: "Clienti",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ProprietarioId",
                table: "Clienti",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Foto",
                table: "Animali",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 29, 234, 72, 73, 51, 128, 128, 180, 53, 5, 61, 104, 224, 177, 162, 12, 184, 157, 162, 228, 64, 249, 10, 201, 126, 211, 22, 191, 217, 215, 158, 109 }, new byte[] { 25, 60, 120, 63, 28, 10, 186, 39, 219, 22, 132, 192, 72, 107, 173, 198 } });

            migrationBuilder.AddForeignKey(
                name: "FK_Clienti_Proprietari_ProprietarioId",
                table: "Clienti",
                column: "ProprietarioId",
                principalTable: "Proprietari",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clienti_Proprietari_ProprietarioId",
                table: "Clienti");

            migrationBuilder.AlterColumn<string>(
                name: "TipologiaAnimale",
                table: "Clienti",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProprietarioId",
                table: "Clienti",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Foto",
                table: "Animali",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 196, 192, 108, 253, 108, 106, 96, 18, 65, 111, 249, 214, 231, 135, 21, 127, 136, 144, 42, 238, 177, 96, 42, 217, 194, 235, 98, 13, 229, 198, 93, 212 }, new byte[] { 183, 76, 233, 96, 160, 164, 152, 45, 209, 147, 91, 100, 1, 249, 95, 184 } });

            migrationBuilder.AddForeignKey(
                name: "FK_Clienti_Proprietari_ProprietarioId",
                table: "Clienti",
                column: "ProprietarioId",
                principalTable: "Proprietari",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
