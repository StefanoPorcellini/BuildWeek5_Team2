namespace ClinicaVeterinaria.Models
{
    public class Ricovero
    {
        public int Id { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime? DataFine { get; set; }
        public int AnimaleId { get; set; }
        public Animale Animale { get; set; }
        public bool Rimborso { get; set; }
        public decimal CostoGiornaliero { get; set; }
        public decimal CostoTotale => DataFine.HasValue
            ? (DataFine.Value - DataInizio).Days * CostoGiornaliero
            : 0;
    }

}
