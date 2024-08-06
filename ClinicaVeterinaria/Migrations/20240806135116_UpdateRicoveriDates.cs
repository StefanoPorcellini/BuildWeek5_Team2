using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRicoveriDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 172, 149, 5, 125, 170, 64, 222, 225, 216, 181, 157, 89, 0, 15, 125, 236, 63, 174, 26, 200, 165, 215, 255, 210, 10, 138, 170, 98, 158, 141, 112, 106 }, new byte[] { 48, 157, 187, 77, 196, 95, 244, 77, 205, 231, 25, 160, 15, 206, 107, 67 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 214, 167, 157, 69, 15, 106, 80, 54, 161, 9, 42, 41, 71, 168, 255, 149, 160, 169, 153, 128, 211, 157, 190, 98, 177, 75, 252, 246, 69, 73, 234, 142 }, new byte[] { 149, 208, 179, 212, 95, 234, 254, 19, 51, 92, 236, 227, 98, 216, 213, 194 } });
        }
    }
}
