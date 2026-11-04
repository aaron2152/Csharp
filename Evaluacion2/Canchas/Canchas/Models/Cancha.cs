using System.ComponentModel.DataAnnotations;

namespace Canchas.Models;

public class Cancha
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60)]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "El tipo es obligatorio.")]
    [StringLength(30)]
    public string Tipo { get; set; } = "";

    [Display(Name = "Precio por hora")]
    [Range(1, 100000000, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal PrecioHora { get; set; }

    public bool Activa { get; set; } = true;
}