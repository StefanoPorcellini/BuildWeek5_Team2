namespace ClinicaVeterinaria.Models
{
    public class Prodotto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Prezzo { get; set; }
        public int NumeroArmadietto { get; set; }
        public int NumeroCassetto { get; set; }
        public string Tipologia { get; set; } // Medicinale o Alimentare

        // Navigazione verso CasaFarmaceutica
        public int CasaFarmaceuticaId { get; set; }
        public CasaFarmaceutica CasaFarmaceutica { get; set; }
    }

}
