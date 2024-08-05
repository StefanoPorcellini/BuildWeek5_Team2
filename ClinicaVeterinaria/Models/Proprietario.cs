namespace ClinicaVeterinaria.Models
{
    public class Proprietario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Telefono { get; set; }
        public string Indirizzo { get; set; }
        public string Citta { get; set; }
        public string CodiceFiscale { get; set; }

        public ICollection<Animale> Animali { get; set; }
    }

}
