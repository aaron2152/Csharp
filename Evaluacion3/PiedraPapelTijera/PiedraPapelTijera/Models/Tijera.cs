
namespace PiedraPapelTijera.Models;

public class Tijera : Jugada
{
    public override string Nombre => "Tijera";

    public override bool GanaA(Jugada otra)
    {
        return otra is Papel;
    }
}
