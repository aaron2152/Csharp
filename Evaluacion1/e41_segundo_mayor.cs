int[] numeros = { 12, 45, 7, 45, 33, 9 };
int mayor = int.MinValue;
int segundo = int.MinValue;

foreach (int n in numeros)
{
    if (n > mayor)
    {
        segundo = mayor;
        mayor = n;
    }
    else if (n > segundo && n < mayor)
    {
        segundo = n;
    }
}

Console.WriteLine("Mayor: " + mayor);
Console.WriteLine("Segundo mayor: " + segundo);