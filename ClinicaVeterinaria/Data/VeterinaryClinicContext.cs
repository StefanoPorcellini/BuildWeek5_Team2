using ClinicaVeterinaria.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class VeterinaryClinicContext : DbContext
{
    public VeterinaryClinicContext(DbContextOptions<VeterinaryClinicContext> options)
        : base(options)
    {
    }

    // DbSet per ogni entità
    public DbSet<Animale> Animali { get; set; }
    public DbSet<Proprietario> Proprietari { get; set; }
    public DbSet<Veterinario> Veterinari { get; set; }
    public DbSet<Farmacista> Farmacisti { get; set; }
    public DbSet<Cliente> Clienti { get; set; }
    public DbSet<Visita> Visite { get; set; }
    public DbSet<Ricovero> Ricoveri { get; set; }
    public DbSet<Prodotto> Prodotti { get; set; }
    public DbSet<CasaFarmaceutica> CaseFarmaceutiche { get; set; }
    public DbSet<Vendita> Vendite { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
