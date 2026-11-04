Console.WriteLine("Dividendo: ");
string a = Console.ReadLine() ?? "";
Console.WriteLine("Divisor: ");
string b = Console.ReadLine() ?? "";

try
{
    int x = int.Parse(a);
    int y = int.Parse(b);
    Console.WriteLine("Resultado: " + (x / y));
}
catch (FormatException)
{
    Console.WriteLine("Error: debes escribir numeros enteros.");
}
catch (DivideByZeroException)
{
    Console.WriteLine("Error: no se puede dividir por cero.");
}
finally
{
    Console.WriteLine("Fin de la operación");
}



