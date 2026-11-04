Console.WriteLine("Escribe una frase: ");
string frase = Console.ReadLine() ?? "";

int vocales = 0;
foreach (char letra in frase.ToLower())
{
    if ("aeiouáéíóú".Contains(letra))
        vocales++;
}

Console.WriteLine("Mayúsculas: " + frase.ToUpper());
Console.WriteLine("Largo: " + frase.Length);
Console.WriteLine("Vocales: " + vocales);