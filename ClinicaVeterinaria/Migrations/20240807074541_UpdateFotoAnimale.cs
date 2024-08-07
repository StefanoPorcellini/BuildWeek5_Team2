using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFotoAnimale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 126, 21, 229, 184, 42, 198, 80, 115, 44, 244, 89, 111, 134, 22, 41, 48, 189, 139, 113, 53, 225, 136, 137, 32, 251, 127, 159, 215, 240, 245, 44, 237 }, new byte[] { 51, 230, 244, 114, 37, 175, 95, 224, 112, 3, 110, 250, 239, 32, 182, 89 } });

            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Animali",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 97, 156, 147, 16, 174, 193, 42, 71, 127, 18, 148, 137, 196, 70, 123, 183, 194, 54, 253, 187, 177, 175, 218, 64, 184, 228, 193, 186, 166, 104, 235, 103 }, new byte[] { 199, 206, 81, 59, 221, 113, 84, 99, 204, 72, 163, 199, 167, 14, 118, 99 } });

            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Animali");

        }
    }
}
