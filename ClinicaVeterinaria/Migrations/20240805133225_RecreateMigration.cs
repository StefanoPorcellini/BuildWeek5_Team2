using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class RecreateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 220, 136, 26, 137, 162, 56, 116, 42, 163, 211, 150, 147, 160, 154, 145, 251, 202, 178, 208, 158, 243, 28, 192, 181, 96, 116, 134, 196, 8, 93, 72, 207 }, new byte[] { 223, 43, 22, 230, 248, 168, 45, 218, 122, 18, 240, 154, 206, 220, 253, 163 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 173, 201, 94, 230, 114, 26, 215, 174, 172, 220, 202, 71, 55, 26, 87, 172, 67, 1, 151, 176, 90, 105, 3, 102, 62, 20, 138, 41, 173, 28, 207, 165 }, new byte[] { 178, 180, 142, 119, 13, 224, 71, 64, 23, 69, 60, 181, 127, 213, 255, 142 } });
        }
    }
}
