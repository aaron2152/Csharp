
namespace PiedraPapelTijera.Models;

public class Piedra : Jugada
{
    public override string Nombre => "Piedra";

    public override bool GanaA(Jugada otra)
    {
        return otra is Tijera;
    }
}
