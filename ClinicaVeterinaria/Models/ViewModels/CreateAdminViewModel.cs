using System.ComponentModel.DataAnnotations;
namespace ClinicaVeterinaria.Models
{
public class CreateAdminViewModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public string Ruolo { get; set; }
}
}

