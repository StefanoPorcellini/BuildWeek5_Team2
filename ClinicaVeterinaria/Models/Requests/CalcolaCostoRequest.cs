namespace ClinicaVeterinaria.Models.Requests
{
    public class CalcolaCostoRequest
    {
        public string DataInizio { get; set; }
        public string DataFine { get; set; }
        public decimal CostoGiornaliero { get; set; }
    }
}
