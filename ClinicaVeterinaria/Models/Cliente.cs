namespace ClinicaVeterinaria.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public int ProprietarioId { get; set; }
        public Proprietario Proprietario { get; set; }
        public string TipologiaAnimale { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Telefono { get; set; }
        public string Indirizzo { get; set; }
        public string Citta { get; set; }
        public string CodiceFiscale { get; set; }
    }


}
