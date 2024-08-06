using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseFarmaceutiche",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Indirizzo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseFarmaceutiche", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Farmacisti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCartellino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmacisti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proprietari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Indirizzo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Citta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodiceFiscale = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proprietari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Ruolo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utenti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veterinari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCartellino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veterinari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prodotti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezzo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumeroArmadietto = table.Column<int>(type: "int", nullable: false),
                    NumeroCassetto = table.Column<int>(type: "int", nullable: false),
                    Tipologia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CasaFarmaceuticaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodotti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prodotti_CaseFarmaceutiche_CasaFarmaceuticaId",
                        column: x => x.CasaFarmaceuticaId,
                        principalTable: "CaseFarmaceutiche",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animali",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipologiaAnimale = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColoreManto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascita = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PossiedeChip = table.Column<bool>(type: "bit", nullable: false),
                    NumeroChip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Randagio = table.Column<bool>(type: "bit", nullable: false),
                    ProprietarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animali", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animali_Proprietari_ProprietarioId",
                        column: x => x.ProprietarioId,
                        principalTable: "Proprietari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clienti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProprietarioId = table.Column<int>(type: "int", nullable: false),
                    TipologiaAnimale = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clienti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clienti_Proprietari_ProprietarioId",
                        column: x => x.ProprietarioId,
                        principalTable: "Proprietari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vendite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodiceFiscaleCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataVendita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProdottoId = table.Column<int>(type: "int", nullable: false),
                    NumeroRicetta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantita = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendite_Prodotti_ProdottoId",
                        column: x => x.ProdottoId,
                        principalTable: "Prodotti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ricoveri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataInizio = table.Column<DateTime>(type: "date", nullable: false),
                    DataFine = table.Column<DateTime>(type: "date", nullable: true),
                    AnimaleId = table.Column<int>(type: "int", nullable: false),
                    Rimborso = table.Column<bool>(type: "bit", nullable: false),
                    CostoGiornaliero = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ricoveri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ricoveri_Animali_AnimaleId",
                        column: x => x.AnimaleId,
                        principalTable: "Animali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Visite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataVisita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnimaleId = table.Column<int>(type: "int", nullable: false),
                    Anamnesi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezzo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visite_Animali_AnimaleId",
                        column: x => x.AnimaleId,
                        principalTable: "Animali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Utenti",
                columns: new[] { "Id", "PasswordHash", "PasswordSalt", "Ruolo", "Username" },
                values: new object[] { 1, new byte[] { 173, 201, 94, 230, 114, 26, 215, 174, 172, 220, 202, 71, 55, 26, 87, 172, 67, 1, 151, 176, 90, 105, 3, 102, 62, 20, 138, 41, 173, 28, 207, 165 }, new byte[] { 178, 180, 142, 119, 13, 224, 71, 64, 23, 69, 60, 181, 127, 213, 255, 142 }, "Admin", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Animali_ProprietarioId",
                table: "Animali",
                column: "ProprietarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Clienti_ProprietarioId",
                table: "Clienti",
                column: "ProprietarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Prodotti_CasaFarmaceuticaId",
                table: "Prodotti",
                column: "CasaFarmaceuticaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ricoveri_AnimaleId",
                table: "Ricoveri",
                column: "AnimaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendite_ProdottoId",
                table: "Vendite",
                column: "ProdottoId");

            migrationBuilder.CreateIndex(
                name: "IX_Visite_AnimaleId",
                table: "Visite",
                column: "AnimaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clienti");

            migrationBuilder.DropTable(
                name: "Farmacisti");

            migrationBuilder.DropTable(
                name: "Ricoveri");

            migrationBuilder.DropTable(
                name: "Utenti");

            migrationBuilder.DropTable(
                name: "Vendite");

            migrationBuilder.DropTable(
                name: "Veterinari");

            migrationBuilder.DropTable(
                name: "Visite");

            migrationBuilder.DropTable(
                name: "Prodotti");

            migrationBuilder.DropTable(
                name: "Animali");

            migrationBuilder.DropTable(
                name: "CaseFarmaceutiche");

            migrationBuilder.DropTable(
                name: "Proprietari");
        }
    }
}
