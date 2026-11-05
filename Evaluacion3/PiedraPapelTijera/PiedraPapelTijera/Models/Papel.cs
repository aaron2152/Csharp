
namespace PiedraPapelTijera.Models;

public class Papel : Jugada
{
    public override string Nombre => "Papel";

    public override bool GanaA(Jugada otra)
    {
        return otra is Piedra;
    }
}
