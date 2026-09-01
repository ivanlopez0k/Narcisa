using Narcisa.BD.Datos.Entity;
using System.ComponentModel.DataAnnotations;

public class RegisterDTO
{
    [Required]
    public string email { get; set; }

    [Required]
    public string password { get; set; }

    [Required]
    public string Nombre { get; set; }


    public string rol { get; set; }
}