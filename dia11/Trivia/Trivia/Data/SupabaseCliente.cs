using System.Net.Http.Json;

namespace Trivia.Data;

public class SupabaseCliente
{
    private readonly HttpClient _http;

    public SupabaseCliente(IConfiguration configuracion)
    {
        string url = configuracion["Supabase:Url"] ?? "";
        string clave = configuracion["Supabase:Key"] ?? "";

        _http = new HttpClient();
        _http.BaseAddress = new Uri(url.TrimEnd('/') + "/rest/v1/");
        _http.DefaultRequestHeaders.Add("apikey", clave);
    }

    public async Task<List<T>> Listar<T>(string consulta)
    {
        HttpResponseMessage respuesta = await _http.GetAsync(consulta);
        respuesta.EnsureSuccessStatusCode();
        return await respuesta.Content.ReadFromJsonAsync<List<T>>() ?? new List<T>();
    }

    public async Task Insertar<T>(string tabla, T objeto)
    {
        HttpResponseMessage respuesta = await _http.PostAsJsonAsync(tabla, objeto);
        respuesta.EnsureSuccessStatusCode();
    }

    public async Task Actualizar<T>(string tabla, int id, T objeto)
    {
        HttpResponseMessage respuesta = await _http.PatchAsJsonAsync(tabla + "?id=eq." + id, objeto);
        respuesta.EnsureSuccessStatusCode();
    }

    public async Task Eliminar(string tabla, int id)
    {
        HttpResponseMessage respuesta = await _http.DeleteAsync(tabla + "?id=eq." + id);
        respuesta.EnsureSuccessStatusCode();
    }
}