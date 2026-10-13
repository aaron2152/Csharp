Console.WriteLine("Conversion de Gs a Dolares");
Console.WriteLine("---------------------------");
Console.WriteLine("Escriba la cotización del dolar");
double cotizaciones = Convert.ToDouble(Console.ReadLine());
Console.WriteLine("Monto en guaranies a convertir; ");
double guaranies = Convert.ToDouble(Console.ReadLine());
double dolares;
dolares = guaranies / cotizaciones;
Console.WriteLine($"Son {dolares:N2} dolares");
