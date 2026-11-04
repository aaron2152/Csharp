int[] temperaturas = { 28, 31, 25, 33, 30, 27 };

int mayor = temperaturas[0];
int menor  = temperaturas[0];
int suma = 0;
foreach (int t in temperaturas)
{
    if (t > mayor) mayor = t;
    if (t < menor) menor = t;
    suma += t;
}

Console.WriteLine("Mayor: " + mayor);
Console.WriteLine("Menor: " + menor);
Console.WriteLine("Promedio: " + (double)suma / temperaturas.Length);