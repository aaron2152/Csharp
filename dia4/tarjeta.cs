Tarjeta tarjeta = new Tarjeta("Lucia Benítez");
tarjeta.Recargar(20000);
tarjeta.Pagar(3400);
tarjeta.Pagar(3400);
tarjeta.Pagar(20000); // Esto mostrará "Saldo insuficiente." porque solo quedan 13.200 Gs.

Console.WriteLine("Titular: " + tarjeta.Titular);
Console.WriteLine("Saldo: " + tarjeta.Saldo + " Gs");

class Tarjeta
{
    private int _saldo;

    // Propiedad de solo lectura para el titular
    public string Titular { get; }

    public int Saldo => _saldo; // Sintaxis abreviada (Expression-bodied) para el getter

    public Tarjeta(string titular)
    {
        Titular = titular; // Se asigna correctamente el parámetro a la propiedad
        _saldo = 0;
    }

    public void Recargar(int monto)
    {
        if (monto <= 0)
        {
            Console.WriteLine("La recarga debe ser mayor a cero.");
            return;
        }
        _saldo += monto;
    }

    public bool Pagar(int monto)
    {
        if (monto > _saldo)
        {
            Console.WriteLine("Saldo insuficiente.");
            return false;
        }
        _saldo -= monto;
        return true;
    }
}