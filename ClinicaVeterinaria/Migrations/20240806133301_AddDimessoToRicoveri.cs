using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class AddDimessoToRicoveri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Dimesso",
                table: "Ricoveri",
                type: "bit",
                nullable: false,
                defaultValue: false);
            migrationBuilder.Sql(
            "UPDATE Ricoveri SET Dimesso = CASE WHEN PrezzoTotale IS NULL THEN 0 ELSE 1 END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimesso",
                table: "Ricoveri");
        }
    }
}
