Console.WriteLine("Año: ");
int anio = int.Parse(Console.ReadLine()!);

bool bisiesto = (anio % 4 == 0 && anio % 100 != 0) || anio % 400 == 0;

if (bisiesto)
    Console.WriteLine(anio + " es bisiesto ");
else
    Console.WriteLine(anio + " no es bisiesto ");