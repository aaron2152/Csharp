Console.WriteLine("Número: ");
int n = int.Parse(Console.ReadLine()!);

Console.WriteLine(n + (EsPrimo(n) ? " es primo." : " no es primo."));

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