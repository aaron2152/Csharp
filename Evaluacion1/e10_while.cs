int suma = 0;
int cantidad = 0;

Console.WriteLine("Escribe un número: (0 para terminar)");
int numero = int.Parse(Console.ReadLine()!);

while (numero != 0)
{
    suma += numero;
    cantidad++;

    Console.WriteLine("Escribe un número: (0 para terminar)");
    numero = int.Parse(Console.ReadLine()!);
}

Console.WriteLine("Cantidad: " + cantidad);
Console.WriteLine("Suma: " + suma);

if (cantidad > 0)
    Console.WriteLine("Promedio: " + (double)suma / cantidad);