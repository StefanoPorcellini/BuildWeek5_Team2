using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models.ViewModels
{
    public class EditProdottoViewModel
    {
        public int Id { get; set; } // ID del prodotto

        // Campi per il prodotto
        [Required(ErrorMessage = "Il nome del prodotto è obbligatorio.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il prezzo è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo deve essere maggiore di zero.")]
        public decimal Prezzo { get; set; }

        [Required(ErrorMessage = "Il numero dell'armadietto è obbligatorio.")]
        public int NumeroArmadietto { get; set; }

        [Required(ErrorMessage = "Il numero del cassetto è obbligatorio.")]
        public int NumeroCassetto { get; set; }

        [Required(ErrorMessage = "La tipologia è obbligatoria.")]
        public string Tipologia { get; set; } // Medicinale o Alimentare

        // Per selezionare la casa farmaceutica
        public int CasaFarmaceuticaId { get; set; } // Id della casa farmaceutica selezionata

        // Lista delle case farmaceutiche esistenti
        public IEnumerable<CasaFarmaceutica> CaseFarmaceutiche { get; set; }
    }
}
