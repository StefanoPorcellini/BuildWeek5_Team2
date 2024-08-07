using ClinicaVeterinaria.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models.ViewModels
{
    public class ProdottoViewModel : IValidatableObject
    {
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

        // Per selezionare o creare una casa farmaceutica
        public int? SelectedCasaFarmaceuticaId { get; set; } // Id della casa farmaceutica selezionata

        // Campi per l'inserimento di una nuova casa farmaceutica
        public string NuovaCasaNome { get; set; }

        public string NuovaCasaIndirizzo { get; set; }

        [EmailAddress(ErrorMessage = "Inserisci un indirizzo email valido.")]
        public string NuovaCasaEmail { get; set; }

        // Lista delle case farmaceutiche esistenti
        public IEnumerable<CasaFarmaceutica> CaseFarmaceutiche { get; set; } = new List<CasaFarmaceutica>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectedCasaFarmaceuticaId.HasValue)
            {
                // Se una casa farmaceutica è stata selezionata, i campi della nuova casa non devono essere obbligatori.
                if (!string.IsNullOrEmpty(NuovaCasaNome) || !string.IsNullOrEmpty(NuovaCasaIndirizzo) || !string.IsNullOrEmpty(NuovaCasaEmail))
                {
                    yield return new ValidationResult("Non è possibile compilare i campi per una nuova casa farmaceutica se una casa farmaceutica esistente è selezionata.", new[] { nameof(SelectedCasaFarmaceuticaId) });
                }
            }
            else
            {
                // Se nessuna casa farmaceutica esistente è selezionata, i campi per la nuova casa farmaceutica sono obbligatori.
                if (string.IsNullOrEmpty(NuovaCasaNome))
                {
                    yield return new ValidationResult("Il nome della nuova casa farmaceutica è obbligatorio.", new[] { nameof(NuovaCasaNome) });
                }

                if (string.IsNullOrEmpty(NuovaCasaIndirizzo))
                {
                    yield return new ValidationResult("L'indirizzo della nuova casa farmaceutica è obbligatorio.", new[] { nameof(NuovaCasaIndirizzo) });
                }

                if (string.IsNullOrEmpty(NuovaCasaEmail))
                {
                    yield return new ValidationResult("L'email della nuova casa farmaceutica è obbligatoria.", new[] { nameof(NuovaCasaEmail) });
                }
            }
        }
    }
}
