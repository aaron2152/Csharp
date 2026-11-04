using System.ComponentModel.DataAnnotations;

namespace Canchas.Models;

public class Reserva
{
    public int Id { get; set; }

    [Display(Name = "Cliente")]
    [Range(1, int.MaxValue, ErrorMessage = "Elige un cliente.")]
    public int ClienteId { get; set; }

    [Display(Name = "Cancha")]
    [Range(1, int.MaxValue, ErrorMessage = "Elige una cancha.")]
    public int CanchaId { get; set; }

    [Display(Name = "Inicio")]
    [Required(ErrorMessage = "Indica la fecha y la hora de inicio.")]
    public DateTime Inicio { get; set; }

    [Display(Name = "Fin")]
    [Required(ErrorMessage = "Indica la fecha y la hora de fin.")]
    public DateTime Fin { get; set; }

    public string Estado { get; set; } = "Confirmada";

    public decimal Total { get; set; }

    // Relaciones: un cliente puede tener muchas reservas y una cancha también.
    // Cada reserva pertenece a UN cliente y a UNA cancha (por eso guarda sus dos llaves).
    public Cliente? Cliente { get; set; }
    public Cancha? Cancha { get; set; }
}