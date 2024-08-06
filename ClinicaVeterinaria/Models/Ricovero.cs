using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models
{
    public class Ricovero
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataInizio { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataFine { get; set; }
        public int AnimaleId { get; set; }
        public Animale? Animale { get; set; }
        public bool Rimborso { get; set; }
        public decimal CostoGiornaliero { get; set; }
        public decimal? PrezzoTotale { get; set; }
        public bool Dimesso { get; set; }  // Nuova proprietà

    }

}
