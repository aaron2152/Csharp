using System.Text.Json.Serialization;

namespace AhorcadoApi.Models
{
    public class Partida
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("palabra")]
        public string Palabra { get; set; } = "";

        [JsonPropertyName("letras_adivinadas")]
        public List<string> LetrasAdivinadas { get; set; } = new();

        [JsonPropertyName("errores")]
        public int Errores { get; set; }

        [JsonPropertyName("max_intentos")]
        public int MaxIntentos { get; set; } = 6;

        [JsonPropertyName("estado")]
        public string Estado { get; set; } = "EnCurso";

        [JsonPropertyName("creada_en")]
        public DateTime CreadaEn { get; set; }
    }
}