// Pelea de Jugadores
Jugador j1 = new Jugador("Fiorella", 50);
Jugador j2 = new Jugador("Maria", 40);

j1.RecibirAtaque(26);
j2.RecibirAtaque(15);
j1.Mostrar();
j2.Mostrar();
Console.WriteLine("15 minutos de descanso");
j1.Curar(15);
j2.Curar(5);
j1.Mostrar();
j2.Mostrar();


class Jugador
{
    public string Nombre;
    public int Vida;

    // Constructor
    public Jugador(string nombre, int vida)
    {
        Nombre = nombre;
        Vida = vida;
    }
    // Metodos
    public void RecibirAtaque(int puntos)
    {
        Vida = Vida - puntos;

    }
    public void Curar(int puntos)
    {
        Vida += puntos;
    }
    public void Mostrar()
    {
        Console.WriteLine($"{Nombre} tiene {Vida} de vida");
    }
}