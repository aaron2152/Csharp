Console.WriteLine("¿En que año naciste?");
string texto = Console.ReadLine() ?? "";

if (int.TryParse(texto, out int anioNacimiento))
{
    int anioActual = 2026;
    Console.WriteLine("Tienes aproximadamente " + (anioActual - anioNacimiento) + " años. ");
}
else
{
    Console.WriteLine("Eso no es un año valido");
}