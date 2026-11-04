Console.WriteLine("Numero: ");
int numero = int.Parse(Console.ReadLine()!);

int resto = numero;
int invertido = 0;

while (resto > 0)
{
    invertido = invertido * 10 + resto % 10;
    resto /= 10;
}

Console.WriteLine("Invertido: " +  invertido);