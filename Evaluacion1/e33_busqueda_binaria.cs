int[] numeros = { 3, 8, 12, 17, 21, 25, 30, 42, 56, 71, 88 };

Console.Write("Número a buscar: ");
int buscado = int.Parse(Console.ReadLine()!);

int inicio = 0;
int fin = numeros.Length - 1;
int posicion = -1;
int pasos = 0;

while (inicio <= fin)
{
    pasos++;
    int medio = (inicio + fin) / 2;

    if (numeros[medio] == buscado)
    {
        posicion = medio;
        break;
    }
    else if (numeros[medio] < buscado)
    {
        inicio = medio + 1;
    }
    else
    {
        fin = medio - 1;
    }
}

if (posicion >= 0)
    Console.WriteLine("Encontrado en la posición " + posicion + " (" + pasos + " pasos).");
else
    Console.WriteLine("No está en la lista (" + pasos + " pasos).");