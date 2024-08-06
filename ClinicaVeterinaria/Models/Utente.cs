using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaVeterinaria.Models
{
    public class Utente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Username { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "La password è obbligatoria.")]
        public string Password { get; set; } // Campo per la password in input

        [Required]
        public byte[]? PasswordHash { get; set; }

        [Required]
        public byte[]? PasswordSalt { get; set; }

        [Required]
        public string Ruolo { get; set; } = "User";

        // Metodo per settare la password
        public void SetPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("La password non può essere vuota", nameof(password));
            }

            PasswordSalt = ClinicaVeterinaria.Service.PasswordService.GenerateSalt();
            PasswordHash = ClinicaVeterinaria.Service.PasswordService.HashPassword(password, PasswordSalt);

            // Verifica che i valori siano stati settati correttamente
            if (PasswordHash == null || PasswordSalt == null)
            {
                throw new InvalidOperationException("PasswordHash e PasswordSalt devono essere settati.");
            }
        }

        // Metodo per verificare la password
        public bool VerifyPassword(string password)
        {
            return ClinicaVeterinaria.Service.PasswordService.VerifyPassword(password, PasswordSalt, PasswordHash);
        }
    }
}
