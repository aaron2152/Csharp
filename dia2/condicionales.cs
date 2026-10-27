using System;
Console.WriteLine("Ingrese su edad");
int edad = Convert.ToInt32(Console.ReadLine());
if (edad >= 18) {
    Console.WriteLine("Bienvenido al Party - Gorillaz");
    if (edad >= 20)
    {
        Console.WriteLine("Desea una bebida?");
    } else 
    {
        Console.WriteLine("Desea una coca?");
    }
}
else
{
    Console.WriteLine("Acceso Denegado");
}