namespace AhorcadoApi.Models
{
    public class RespuestaPartidaDto
    {
        public Guid Id { get; set; }

        public string PalabraOculta { get; set; } = "";

        public List<string> LetrasAdivinadas { get; set; } = new();

        public int Errores { get; set; }

        public int IntentosRestantes { get; set; }

        public string Estado { get; set; } = "";

        public string? PalabraCorrecta { get; set; }
    }
}