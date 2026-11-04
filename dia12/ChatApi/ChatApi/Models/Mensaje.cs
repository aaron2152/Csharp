using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatApi.Models;

public class Mensaje
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Usuario { get; set; } = "";

    [Required]
    [StringLength(280)]
    public string Texto { get; set; } = "";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime Creado { get; set; }
}