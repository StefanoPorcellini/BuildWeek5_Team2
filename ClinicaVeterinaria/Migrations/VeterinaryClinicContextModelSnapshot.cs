﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClinicaVeterinaria.Migrations
{
    [DbContext(typeof(VeterinaryClinicContext))]
    partial class VeterinaryClinicContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClinicaVeterinaria.Models.Animale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ColoreManto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DataNascita")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroChip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PossiedeChip")
                        .HasColumnType("bit");

                    b.Property<int?>("ProprietarioId")
                        .HasColumnType("int");

                    b.Property<bool>("Randagio")
                        .HasColumnType("bit");

                    b.Property<string>("TipologiaAnimale")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProprietarioId");

                    b.ToTable("Animali");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.CasaFarmaceutica", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Indirizzo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CaseFarmaceutiche");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProprietarioId")
                        .HasColumnType("int");

                    b.Property<string>("TipologiaAnimale")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProprietarioId");

                    b.ToTable("Clienti");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Farmacista", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("IdCartellino")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Farmacisti");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Prodotto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CasaFarmaceuticaId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumeroArmadietto")
                        .HasColumnType("int");

                    b.Property<int>("NumeroCassetto")
                        .HasColumnType("int");

                    b.Property<decimal>("Prezzo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Tipologia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CasaFarmaceuticaId");

                    b.ToTable("Prodotti");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Proprietario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Citta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodiceFiscale")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Indirizzo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Proprietari");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Ricovero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnimaleId")
                        .HasColumnType("int");

                    b.Property<decimal>("CostoGiornaliero")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("DataFine")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataInizio")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Dimesso")
                        .HasColumnType("bit");

                    b.Property<decimal?>("PrezzoTotale")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Rimborso")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AnimaleId");

                    b.ToTable("Ricoveri");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Utente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Ruolo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Utenti");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PasswordHash = new byte[] { 214, 167, 157, 69, 15, 106, 80, 54, 161, 9, 42, 41, 71, 168, 255, 149, 160, 169, 153, 128, 211, 157, 190, 98, 177, 75, 252, 246, 69, 73, 234, 142 },
                            PasswordSalt = new byte[] { 149, 208, 179, 212, 95, 234, 254, 19, 51, 92, 236, 227, 98, 216, 213, 194 },
                            Ruolo = "Admin",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Vendita", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CodiceFiscaleCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataVendita")
                        .HasColumnType("datetime2");

                    b.Property<string>("NumeroRicetta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProdottoId")
                        .HasColumnType("int");

                    b.Property<int>("Quantita")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProdottoId");

                    b.ToTable("Vendite");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Veterinario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("IdCartellino")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Veterinari");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Visita", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Anamnesi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AnimaleId")
                        .HasColumnType("int");

                    b.Property<string>("Cura")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataVisita")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Prezzo")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("AnimaleId");

                    b.ToTable("Visite");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Animale", b =>
                {
                    b.HasOne("ClinicaVeterinaria.Models.Proprietario", "Proprietario")
                        .WithMany("Animali")
                        .HasForeignKey("ProprietarioId");

                    b.Navigation("Proprietario");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Cliente", b =>
                {
                    b.HasOne("ClinicaVeterinaria.Models.Proprietario", "Proprietario")
                        .WithMany()
                        .HasForeignKey("ProprietarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Proprietario");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Prodotto", b =>
                {
                    b.HasOne("ClinicaVeterinaria.Models.CasaFarmaceutica", "CasaFarmaceutica")
                        .WithMany("Prodotti")
                        .HasForeignKey("CasaFarmaceuticaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CasaFarmaceutica");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Ricovero", b =>
                {
                    b.HasOne("ClinicaVeterinaria.Models.Animale", "Animale")
                        .WithMany()
                        .HasForeignKey("AnimaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animale");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Vendita", b =>
                {
                    b.HasOne("ClinicaVeterinaria.Models.Prodotto", "Prodotto")
                        .WithMany()
                        .HasForeignKey("ProdottoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Prodotto");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Visita", b =>
                {
                    b.HasOne("ClinicaVeterinaria.Models.Animale", "Animale")
                        .WithMany()
                        .HasForeignKey("AnimaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animale");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.CasaFarmaceutica", b =>
                {
                    b.Navigation("Prodotti");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Models.Proprietario", b =>
                {
                    b.Navigation("Animali");
                });
#pragma warning restore 612, 618
        }
    }
}
