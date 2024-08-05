namespace ClinicaVeterinaria.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public int ProprietarioId { get; set; }
        public Proprietario Proprietario { get; set; }
        public string TipologiaAnimale { get; set; }
    }

}
