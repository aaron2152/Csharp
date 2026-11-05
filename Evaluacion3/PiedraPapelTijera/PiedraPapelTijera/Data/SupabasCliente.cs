using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PiedraPapelTijera.Data;

/// <summary>
/// Habla con Supabase por su API REST. Es el único lugar del proyecto que conoce la dirección
/// y la clave de acceso. Las claves se leen de la configuración: nunca se escriben aquí.
/// </summary>
public class SupabaseCliente
{
    private readonly HttpClient _http;

    // Las columnas están en snake_case y las propiedades de C# en PascalCase.
    private static readonly JsonSerializerOptions Opciones = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public SupabaseCliente(IConfiguration configuracion)
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

    /// <summary>Lee filas. Ejemplo: "puntajes?select=*&amp;order=puntos.desc&amp;limit=10".</summary>
    public async Task<List<T>> Listar<T>(string consulta)
    {
        HttpResponseMessage respuesta = await _http.GetAsync(consulta);
        await Verificar(respuesta);
        return await respuesta.Content.ReadFromJsonAsync<List<T>>(Opciones) ?? new List<T>();
    }

    /// <summary>Guarda una fila nueva en la tabla indicada.</summary>
    public async Task Insertar<T>(string tabla, T fila)
    {
        string json = JsonSerializer.Serialize(fila, Opciones);
        HttpResponseMessage respuesta = await _http.PostAsync(tabla, new StringContent(json, Encoding.UTF8, "application/json"));
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