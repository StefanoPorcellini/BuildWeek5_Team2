using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaVeterinaria.Migrations
{
        /// <inheritdoc />
        public partial class UpdateRicoveriDatesManual : Migration
        {
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.AlterColumn<DateTime>(
                    name: "DataInizio",
                    table: "Ricoveri",
                    type: "date",
                    nullable: false,
                    oldClrType: typeof(DateTime),
                    oldType: "datetime2");

                migrationBuilder.AlterColumn<DateTime>(
                    name: "DataFine",
                    table: "Ricoveri",
                    type: "date",
                    nullable: true,
                    oldClrType: typeof(DateTime),
                    oldType: "datetime2",
                    oldNullable: true);
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.AlterColumn<DateTime>(
                    name: "DataInizio",
                    table: "Ricoveri",
                    type: "datetime2",
                    nullable: false,
                    oldClrType: typeof(DateTime),
                    oldType: "date");

                migrationBuilder.AlterColumn<DateTime>(
                    name: "DataFine",
                    table: "Ricoveri",
                    type: "datetime2",
                    nullable: true,
                    oldClrType: typeof(DateTime),
                    oldType: "date",
                    oldNullable: true);
            }
        }

    }
