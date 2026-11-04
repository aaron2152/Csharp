using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Canchas.Models;

namespace Canchas.Data;

/// <summary>
/// Lo mismo que MySqlRepositorio, pero contra Supabase (PostgreSQL en internet), usando su API REST.
/// </summary>
public class SupabaseRepositorio : IRepositorio
{
    // Trae la reserva junto con su cliente y su cancha (equivale al JOIN de MySQL).
    private const string ConRelaciones = "reservas?select=*,cliente:clientes(*),cancha:canchas(*)";

    // Las columnas están en snake_case (cliente_id) y las propiedades de C# en PascalCase (ClienteId).
    private static readonly JsonSerializerOptions Opciones = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _http;

    public SupabaseRepositorio(IConfiguration configuracion)
    {
        string url = configuracion["Supabase:Url"] ?? "";
        string clave = configuracion["Supabase:Key"] ?? "";

        if (!url.StartsWith("http") || clave == "")
            throw new InvalidOperationException("Falta configurar Supabase:Url y Supabase:Key.");

        if (clave.StartsWith("sb_publishable_"))
            throw new InvalidOperationException("Supabase:Key es una clave pública. Usa la Secret key (sb_secret_...).");

        _http = new HttpClient { BaseAddress = new Uri(url.TrimEnd('/') + "/rest/v1/") };
        _http.DefaultRequestHeaders.Add("apikey", clave);
    }

    // ---------------------------------------------------------- Lecturas

    public Task<List<Reserva>> ListarReservas()
    {
        return Leer<Reserva>(ConRelaciones + "&order=inicio");
    }

    public async Task<Reserva?> ObtenerReserva(int id)
    {
        List<Reserva> lista = await Leer<Reserva>(ConRelaciones + "&id=eq." + id);
        return lista.FirstOrDefault();
    }

    public Task<List<Cliente>> ListarClientes()
    {
        return Leer<Cliente>("clientes?select=*&order=nombre");
    }

    public Task<List<Cancha>> ListarCanchas()
    {
        return Leer<Cancha>("canchas?select=*&order=nombre");
    }

    public async Task<Cancha?> ObtenerCancha(int id)
    {
        List<Cancha> lista = await Leer<Cancha>("canchas?select=*&id=eq." + id);
        return lista.FirstOrDefault();
    }

    public async Task<bool> HayConflicto(int canchaId, DateTime inicio, DateTime fin, int ignorarReservaId)
    {
        // Mismo criterio que en MySQL: dos horarios se cruzan si cada uno empieza antes de que el otro termine.
        string consulta = "reservas?select=id"
            + "&cancha_id=eq." + canchaId
            + "&estado=neq.Cancelada"
            + "&inicio=lt." + Fecha(fin)
            + "&fin=gt." + Fecha(inicio)
            + "&id=neq." + ignorarReservaId;

        List<Reserva> cruzadas = await Leer<Reserva>(consulta);
        return cruzadas.Count > 0;
    }

    // ---------------------------------------------------------- Escrituras

    public Task CrearReserva(Reserva reserva)
    {
        return Enviar(HttpMethod.Post, "reservas", Columnas(reserva));
    }

    public Task ActualizarReserva(Reserva reserva)
    {
        return Enviar(HttpMethod.Patch, "reservas?id=eq." + reserva.Id, Columnas(reserva));
    }

    public Task EliminarReserva(int id)
    {
        return Enviar(HttpMethod.Delete, "reservas?id=eq." + id, null);
    }

    // ---------------------------------------------------------- Apoyo

    // Solo las columnas propias de la tabla: el cliente y la cancha se guardan por su llave.
    private static object Columnas(Reserva r)
    {
        return new { r.ClienteId, r.CanchaId, r.Inicio, r.Fin, r.Estado, r.Total };
    }

    private static string Fecha(DateTime fecha)
    {
        return Uri.EscapeDataString(fecha.ToString("yyyy-MM-dd'T'HH:mm:ss"));
    }

    private async Task<List<T>> Leer<T>(string consulta)
    {
        HttpResponseMessage respuesta = await _http.GetAsync(consulta);
        await Verificar(respuesta);
        return await respuesta.Content.ReadFromJsonAsync<List<T>>(Opciones) ?? new List<T>();
    }

    private async Task Enviar(HttpMethod metodo, string ruta, object? cuerpo)
    {
        HttpRequestMessage pedido = new HttpRequestMessage(metodo, ruta);
        if (cuerpo != null)
            pedido.Content = new StringContent(JsonSerializer.Serialize(cuerpo, Opciones), Encoding.UTF8, "application/json");

        HttpResponseMessage respuesta = await _http.SendAsync(pedido);
        await Verificar(respuesta);
    }

    private static async Task Verificar(HttpResponseMessage respuesta)
    {
        if (respuesta.IsSuccessStatusCode)
            return;

        string detalle = await respuesta.Content.ReadAsStringAsync();
        throw new InvalidOperationException("Supabase respondió " + (int)respuesta.StatusCode + ": " + detalle);
    }
}