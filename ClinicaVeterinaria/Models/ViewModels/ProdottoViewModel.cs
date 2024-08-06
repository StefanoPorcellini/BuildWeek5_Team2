using ClinicaVeterinaria.Models;
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
        public decimal Prezzo { get; set; }

        [Required(ErrorMessage = "Il numero armadietto è obbligatorio.")]
        public int NumeroArmadietto { get; set; }

        [Required(ErrorMessage = "Il numero cassetto è obbligatorio.")]
        public int NumeroCassetto { get; set; }

        [Required(ErrorMessage = "La tipologia è obbligatoria.")]
        public string Tipologia { get; set; }

        // Per selezionare o creare una casa farmaceutica
        public int? SelectedCasaFarmaceuticaId { get; set; }

        // Campi per l'inserimento di una nuova casa farmaceutica
        public string NuovaCasaNome { get; set; }
        public string NuovaCasaIndirizzo { get; set; }
        public string NuovaCasaEmail { get; set; }

        // Lista delle case farmaceutiche esistenti
        public IEnumerable<CasaFarmaceutica> CaseFarmaceutiche { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SelectedCasaFarmaceuticaId.HasValue)
            {
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
