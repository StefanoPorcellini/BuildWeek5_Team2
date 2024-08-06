namespace ClinicaVeterinaria.Models
{
    public class Animale
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string TipologiaAnimale { get; set; } 
        public string ColoreManto { get; set; }
        public DateTime? DataNascita { get; set; }
        public bool PossiedeChip { get; set; }
        public string NumeroChip { get; set; } 
        public bool Randagio { get; set; }
        public int ProprietarioId { get; set; }
        public Proprietario? Proprietario { get; set; }
    }

}
