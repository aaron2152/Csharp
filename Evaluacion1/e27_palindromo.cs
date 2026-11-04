Console.Write("Frase: ");
string frase = Console.ReadLine() ?? "";
string limpia = "";

foreach (char c in frase.ToLower())
{
    if (char.IsLetterOrDigit(c))
        limpia += c;
}

bool esPalindromo = true;
for (int i = 0; i < limpia.Length / 2; i++)
{
    if (limpia[i] != limpia[limpia.Length - 1 - i])
    {
        esPalindromo = false;
        break;
    }
}

Console.WriteLine(esPalindromo ? "Es un palíndromo." : "No es un palíndromo.");