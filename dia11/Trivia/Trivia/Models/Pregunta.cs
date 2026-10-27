using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trivia.Models;

public class Pregunta
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Escribe la pregunta.")]
    public string Texto { get; set; } = "";

    [JsonPropertyName("opcion_a")]
    [Display(Name = "Opción A")]
    [Required(ErrorMessage = "Escribe la opción A.")]
    public string OpcionA { get; set; } = "";

    [JsonPropertyName("opcion_b")]
    [Display(Name = "Opción B")]
    [Required(ErrorMessage = "Escribe la opción B.")]
    public string OpcionB { get; set; } = "";

    [JsonPropertyName("opcion_c")]
    [Display(Name = "Opción C")]
    [Required(ErrorMessage = "Escribe la opción C.")]
    public string OpcionC { get; set; } = "";

    [JsonPropertyName("opcion_d")]
    [Display(Name = "Opción D")]
    [Required(ErrorMessage = "Escribe la opción D.")]
    public string OpcionD { get; set; } = "";

    [Display(Name = "Respuesta correcta")]
    [RegularExpression("^[ABCD]$", ErrorMessage = "La respuesta correcta debe ser A, B, C o D.")]
    public string Correcta { get; set; } = "A";
}