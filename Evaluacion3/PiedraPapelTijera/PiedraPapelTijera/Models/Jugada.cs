
namespace PiedraPapelTijera.Models;

public abstract class Jugada
{
    public abstract string Nombre { get; }

    public abstract bool GanaA(Jugada otra);
}
