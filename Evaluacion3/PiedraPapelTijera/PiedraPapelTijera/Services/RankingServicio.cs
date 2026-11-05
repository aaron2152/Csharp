
using PiedraPapelTijera.Data;
using PiedraPapelTijera.Models;

namespace PiedraPapelTijera.Services;

public class RankingServicio
{
    private readonly SupabaseCliente _supabase;

    public RankingServicio(SupabaseCliente supabase)
    {
        _supabase = supabase;
    }

    // Consultar los 10 mejores puntajes
    public async Task<List<Puntaje>> ListarMejores()
    {
        string consulta =
            "puntajes?select=id,jugador,puntos,creado" +
            "&order=puntos.desc,creado.asc" +
            "&limit=10";

        return await _supabase.Listar<Puntaje>(consulta);
    }

    // Guardar el puntaje final
    public async Task GuardarPuntaje(
        string jugador,
        int puntos)
    {
        if (string.IsNullOrWhiteSpace(jugador) ||
            jugador.Trim().Length > 20)
        {
            throw new ArgumentException(
                "El nombre del jugador no es válido.");
        }

        if (puntos < 0)
        {
            throw new ArgumentException(
                "El puntaje no puede ser negativo.");
        }

        Puntaje nuevoPuntaje = new Puntaje
        {
            Jugador = jugador.Trim(),
            Puntos = puntos
        };

        await _supabase.Insertar(
            "puntajes",
            nuevoPuntaje
        );
    }
}
