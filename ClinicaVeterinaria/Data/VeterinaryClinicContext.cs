using ClinicaVeterinaria.Models;
using Microsoft.EntityFrameworkCore;
using ClinicaVeterinaria.Service;

public class VeterinaryClinicContext : DbContext
{
    public VeterinaryClinicContext(DbContextOptions<VeterinaryClinicContext> options)
        : base(options)
    {
    }

    // DbSet per ogni entità
    public DbSet<Utente> Utenti { get; set; }
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
        base.OnModelCreating(modelBuilder);

        // Configurazione delle proprietà decimal
        modelBuilder.Entity<Prodotto>()
            .Property(p => p.Prezzo)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Ricovero>()
            .Property(r => r.CostoGiornaliero)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Visita>()
            .Property(v => v.Prezzo)
            .HasColumnType("decimal(18,2)");

        // Configurazione del seed per l'utente Admin
        var salt = PasswordService.GenerateSalt();
        var hashedPassword = PasswordService.HashPassword("Admin1234!", salt);

        modelBuilder.Entity<Utente>().HasData(new Utente
        {
            Id = 1, // ID univoco
            Username = "admin",
            PasswordHash = hashedPassword,
            PasswordSalt = salt, // Nome corretto della proprietà
            Ruolo = "Admin" // Nome corretto della proprietà
        });
    }
}
