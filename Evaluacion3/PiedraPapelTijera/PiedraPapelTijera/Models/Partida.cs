
namespace PiedraPapelTijera.Models;

public class Partida
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string NombreJugador { get; set; } = "";

    public int Fichas { get; set; } = 100;

    public int RondasJugadas { get; set; } = 0;

    public bool Finalizada { get; set; } = false;

    public string Estado { get; set; } = "En curso";
}
