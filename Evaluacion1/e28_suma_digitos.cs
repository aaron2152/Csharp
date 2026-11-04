Console.WriteLine("Número: ");
int numero = int.Parse(Console.ReadLine()!);

int suma = 0;
int resto = Math.Abs(numero);

while (resto > 0)
{
    suma += resto % 10;
    resto /= 10;
}

Console.WriteLine("La suma de los dígitos es " + suma + ".");