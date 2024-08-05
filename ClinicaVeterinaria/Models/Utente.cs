using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models
{

    public class Utente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Username { get; set; }

        [Required, StringLength(20)]
        public string Password { get; set; }

        [Required]
        public string Ruolo { get; set; } = "User";
    }

}
