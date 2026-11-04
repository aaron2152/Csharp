int[] denominaciones = { 100000, 50000, 20000, 10000, 5000, 2000, 1000, 500, 100, 50 };

Console.Write("Monto en guaraníes: ");
int monto = int.Parse(Console.ReadLine()!);

foreach (int valor in denominaciones)
{
    int cantidad = monto / valor;
    if (cantidad > 0)
    {
        Console.WriteLine(cantidad + " x " + valor);
        monto %= valor;
    }
}

if (monto > 0)
    Console.WriteLine("Sobran " + monto + " Gs. que no se pueden entregar.");