Console.WriteLine("Escribe un numero entero positivo: ");
int numero = int.Parse(Console.ReadLine()!);

string binario = "";
int resto = numero;

if (resto == 0)
    binario = "0";

while (resto > 0)
{
    binario = (resto % 2) + binario;
    resto /= 2;
}

Console.WriteLine("Binario: " + binario);
Console.WriteLine("Comprobación: " + Convert.ToString(numero, 2)); 