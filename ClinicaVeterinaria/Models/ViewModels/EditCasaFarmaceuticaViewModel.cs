using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models.ViewModels
{
    public class EditCasaFarmaceuticaViewModel
    {
        public int Id { get; set; } // ID della casa farmaceutica

        [Required(ErrorMessage = "Il nome della casa farmaceutica è obbligatorio.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "L'indirizzo è obbligatorio.")]
        public string Indirizzo { get; set; }

        [EmailAddress(ErrorMessage = "Inserisci un indirizzo email valido.")]
        public string Email { get; set; }
    }
}
