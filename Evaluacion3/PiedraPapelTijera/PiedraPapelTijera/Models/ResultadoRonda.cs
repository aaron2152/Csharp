
namespace PiedraPapelTijera.Models;

public class ResultadoRonda
{
    public string JugadaJugador { get; set; } = "";

    public string JugadaComputadora { get; set; } = "";

    public int Apuesta { get; set; }

    public string Resultado { get; set; } = "";

    public int FichasFinales { get; set; }
}
