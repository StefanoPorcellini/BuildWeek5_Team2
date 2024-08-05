namespace ClinicaVeterinaria.Models
{
    public class Vendita
    {
        public int Id { get; set; }
        public string CodiceFiscaleCliente { get; set; }
        public DateTime DataVendita { get; set; }
        public int ProdottoId { get; set; }
        public Prodotto Prodotto { get; set; }
        public string NumeroRicetta { get; set; } // Nullable se non è richiesto
        public int Quantita { get; set; }
    }

}
