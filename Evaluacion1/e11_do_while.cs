int numero;

do
{
    Console.WriteLine("Elige un número del 1 al 10: ");
    numero = int.Parse(Console.ReadLine()!);

    if (numero < 1 || numero > 10)
        Console.WriteLine("Fuera de rango, intenta de nuevo ");
}
while (numero < 1 || numero > 10);

Console.WriteLine("Elegiste el número " +  numero + ".");