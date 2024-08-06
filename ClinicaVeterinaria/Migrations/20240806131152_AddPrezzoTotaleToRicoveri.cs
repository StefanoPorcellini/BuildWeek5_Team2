using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class AddPrezzoTotaleToRicoveri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animali_Proprietari_ProprietarioId",
                table: "Animali");

            migrationBuilder.AddColumn<decimal>(
                name: "PrezzoTotale",
                table: "Ricoveri",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProprietarioId",
                table: "Animali",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroChip",
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
                values: new object[] { new byte[] { 224, 52, 11, 200, 107, 38, 194, 242, 114, 250, 145, 180, 191, 61, 177, 108, 199, 1, 140, 238, 252, 17, 182, 160, 133, 237, 252, 166, 94, 170, 51, 0 }, new byte[] { 225, 247, 156, 59, 78, 112, 246, 229, 211, 170, 5, 237, 189, 205, 209, 233 } });

            migrationBuilder.AddForeignKey(
                name: "FK_Animali_Proprietari_ProprietarioId",
                table: "Animali",
                column: "ProprietarioId",
                principalTable: "Proprietari",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animali_Proprietari_ProprietarioId",
                table: "Animali");

            migrationBuilder.DropColumn(
                name: "PrezzoTotale",
                table: "Ricoveri");

            migrationBuilder.AlterColumn<int>(
                name: "ProprietarioId",
                table: "Animali",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeroChip",
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
                values: new object[] { new byte[] { 220, 136, 26, 137, 162, 56, 116, 42, 163, 211, 150, 147, 160, 154, 145, 251, 202, 178, 208, 158, 243, 28, 192, 181, 96, 116, 134, 196, 8, 93, 72, 207 }, new byte[] { 223, 43, 22, 230, 248, 168, 45, 218, 122, 18, 240, 154, 206, 220, 253, 163 } });

            migrationBuilder.AddForeignKey(
                name: "FK_Animali_Proprietari_ProprietarioId",
                table: "Animali",
                column: "ProprietarioId",
                principalTable: "Proprietari",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
