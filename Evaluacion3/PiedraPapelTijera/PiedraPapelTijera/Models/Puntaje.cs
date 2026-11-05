using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PiedraPapelTijera.Models;

public class Puntaje
{
    // Los asigna la base de datos: no se envían al guardar.
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Jugador { get; set; } = "";

    public int Puntos { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime Creado { get; set; }
}