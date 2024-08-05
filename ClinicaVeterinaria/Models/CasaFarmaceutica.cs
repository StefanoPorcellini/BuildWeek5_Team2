namespace ClinicaVeterinaria.Models
{
    public class CasaFarmaceutica
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Indirizzo { get; set; }
        public string Email { get; set; }

        public ICollection<Prodotto> Prodotti { get; set; }
    }

}
