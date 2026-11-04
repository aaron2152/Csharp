using Microsoft.AspNetCore.Mvc;
using AhorcadoApi.Models;
using AhorcadoApi.Data;

namespace AhorcadoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidasController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public PartidasController(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        // ====================================
        // CREAR NUEVA PARTIDA
        // POST: api/partidas
        // ====================================

        [HttpPost]
        public async Task<IActionResult> CrearPartida(
            [FromBody] CrearPartidaDto dto)
        {
            var random = new Random();

            string dificultad = (dto.Dificultad ?? "Normal")
                .Trim()
                .ToLowerInvariant();

            int maxIntentos = dificultad switch
            {
                "facil" => 8,
                "normal" => 6,
                "dificil" => 4,
                _ => 6
            };

            string palabra = Palabras.Lista[
                random.Next(Palabras.Lista.Length)
            ];

            var partida = new Partida
            {
                Id = Guid.NewGuid(),
                Palabra = palabra,
                LetrasAdivinadas = new List<string>(),
                Errores = 0,
                MaxIntentos = maxIntentos,
                Estado = "EnCurso",
                CreadaEn = DateTime.UtcNow
            };

            var creada = await _supabase.CrearPartida(partida);

            return Ok(ConvertirRespuesta(creada));
        }

        // ====================================
        // OBTENER PARTIDA
        // GET: api/partidas/{id}
        // ====================================

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPartida(Guid id)
        {
            var partida = await _supabase.ObtenerPartida(id);

            if (partida == null)
                return NotFound(new
                {
                    mensaje = "Partida no encontrada"
                });

            return Ok(ConvertirRespuesta(partida));
        }

        // ====================================
        // ADIVINAR LETRA
        // POST: api/partidas/{id}/adivinar
        // ====================================

        [HttpPost("{id}/adivinar")]
        public async Task<IActionResult> AdivinarLetra(
            Guid id,
            [FromBody] AdivinarLetraDto dto)
        {
            var partida = await _supabase.ObtenerPartida(id);

            if (partida == null)
                return NotFound(new
                {
                    mensaje = "Partida no encontrada"
                });

            if (partida.Estado != "EnCurso")
                return BadRequest(new
                {
                    mensaje = "La partida ya ha finalizado"
                });

            if (string.IsNullOrWhiteSpace(dto.Letra) ||
                dto.Letra.Trim().Length != 1 ||
                !char.IsLetter(dto.Letra.Trim()[0]))
            {
                return BadRequest(new
                {
                    mensaje = "Debes ingresar una sola letra válida"
                });
            }

            string letra = dto.Letra.Trim().ToUpperInvariant();

            if (partida.LetrasAdivinadas.Contains(letra))
            {
                return BadRequest(new
                {
                    mensaje = "Ya has utilizado esa letra"
                });
            }

            partida.LetrasAdivinadas.Add(letra);

            // Comprobar si la letra existe
            if (!partida.Palabra.Contains(letra))
            {
                partida.Errores++;
            }

            // Comprobar si ganó
            bool gano = partida.Palabra
                .All(c => partida.LetrasAdivinadas.Contains(c.ToString()));

            if (gano)
            {
                partida.Estado = "Ganada";
            }
            else if (partida.Errores >= partida.MaxIntentos)
            {
                partida.Estado = "Perdida";
            }

            await _supabase.ActualizarPartida(partida);

            return Ok(ConvertirRespuesta(partida));
        }

        // ====================================
        // CONVERTIR RESPUESTA
        // ====================================

        private static RespuestaPartidaDto ConvertirRespuesta(
            Partida partida)
        {
            string palabraOculta = string.Join(" ",
                partida.Palabra.Select(letra =>
                    partida.LetrasAdivinadas.Contains(
                        letra.ToString())
                        ? letra.ToString()
                        : "_"
                )
            );

            return new RespuestaPartidaDto
            {
                Id = partida.Id,
                PalabraOculta = palabraOculta,
                LetrasAdivinadas = partida.LetrasAdivinadas,
                Errores = partida.Errores,
                IntentosRestantes =
                    partida.MaxIntentos - partida.Errores,
                Estado = partida.Estado,

                PalabraCorrecta =
                    partida.Estado == "Perdida" ||
                    partida.Estado == "Ganada"
                        ? partida.Palabra
                        : null
            };
        }
    }
}