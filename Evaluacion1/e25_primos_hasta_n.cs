Console.Write("Hasta qué número: ");
int limite = int.Parse(Console.ReadLine()!);
int cantidad = 0;

for (int i = 2; i <= limite; i++)
{
    if (EsPrimo(i))
    {
        Console.Write(i + " ");
        cantidad++;
    }
}

Console.WriteLine();
Console.WriteLine("Total de primos: " + cantidad);

bool EsPrimo(int numero)
{
    if (numero < 2)
        return false;

    for (int divisor = 2; divisor * divisor <= numero; divisor++)
    {
        if (numero % divisor == 0)
            return false;
    }

    return true;
}