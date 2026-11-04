using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AhorcadoApi.Models;

namespace AhorcadoApi.Data
{
    public class SupabaseService
    {
        private readonly HttpClient _http;
        private readonly string _url;

        public SupabaseService(
            HttpClient http,
            IConfiguration configuration)
        {
            _http = http;

            _url = configuration["Supabase:Url"]
                ?? throw new Exception("Falta configurar Supabase:Url");

            string key = configuration["Supabase:Key"]
                ?? throw new Exception("Falta configurar Supabase:Key");

            _http.DefaultRequestHeaders.Add("apikey", key);

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", key);
        }

        // Crear una partida
        public async Task<Partida> CrearPartida(Partida partida)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_url}/rest/v1/partidas"
            );

            request.Headers.Add("Prefer", "return=representation");

            request.Content = JsonContent.Create(partida);

            var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var resultado = await response.Content
                .ReadFromJsonAsync<List<Partida>>();

            return resultado?.FirstOrDefault()
                ?? throw new Exception("No se pudo crear la partida");
        }
        // Buscar partida por ID
        public async Task<Partida?> ObtenerPartida(Guid id)
        {
            string endpoint =
                $"{_url}/rest/v1/partidas?id=eq.{id}&select=*";

            var response = await _http.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();

            var partidas = await response.Content
                .ReadFromJsonAsync<List<Partida>>();

            return partidas?.FirstOrDefault();
        }

        // Actualizar partida
        public async Task ActualizarPartida(Partida partida)
        {
            string endpoint =
                $"{_url}/rest/v1/partidas?id=eq.{partida.Id}";

            var datos = new Dictionary<string, object>
            {
                ["letras_adivinadas"] = partida.LetrasAdivinadas,
                ["errores"] = partida.Errores,
                ["estado"] = partida.Estado
            };

            var response = await _http.PatchAsync(
                endpoint,
                new StringContent(
                    JsonSerializer.Serialize(datos),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            response.EnsureSuccessStatusCode();
        }
    }
}