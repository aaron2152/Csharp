using System.ComponentModel.DataAnnotations;

namespace Canchas.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(80)]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [StringLength(30)]
    public string Telefono { get; set; } = "";

    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    [StringLength(120)]
    public string? Email { get; set; }
}