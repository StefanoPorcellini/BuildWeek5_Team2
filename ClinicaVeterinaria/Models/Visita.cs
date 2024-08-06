namespace ClinicaVeterinaria.Models
{
    public class Visita
    {
        public int Id { get; set; }
        public DateTime DataVisita { get; set; } = DateTime.Now;
        public int AnimaleId { get; set; }
        public Animale? Animale { get; set; }
        public string Anamnesi { get; set; }
        public string Cura { get; set; }
        public decimal Prezzo { get; set; }
    }

}
