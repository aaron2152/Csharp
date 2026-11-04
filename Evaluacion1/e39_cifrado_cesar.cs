Console.Write("Mensaje: ");
string mensaje = (Console.ReadLine() ?? "").ToUpper();
int desplazamiento = 3;

string cifrado = "";
foreach (char c in mensaje)
{
    if (c >= 'A' && c <= 'Z')
        cifrado += (char)((c - 'A' + desplazamiento) % 26 + 'A');
    else
        cifrado += c;
}

string descifrado = "";
foreach (char c in cifrado)
{
    if (c >= 'A' && c <= 'Z')
        descifrado += (char)((c - 'A' - desplazamiento + 26) % 26 + 'A');
    else
        descifrado += c;
}

Console.WriteLine("Cifrado:    " + cifrado);
Console.WriteLine("Descifrado: " + descifrado);