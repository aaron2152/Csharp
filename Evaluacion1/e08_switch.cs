Console.WriteLine("Numero de dia (1 al 7): ");
int dia = int.Parse(Console.ReadLine()!);
string nombre;
switch (dia)
{
    case 1: nombre = "Lunes"; break;
    case 2: nombre = "Martes"; break;
    case 3: nombre = "Miércoles"; break;
    case 4: nombre = "Jueves"; break;
    case 5: nombre = "Viernes"; break;
    case 6: nombre = "Sábado"; break;
    case 7: nombre = "Domingo"; break;
    default:
        Console.WriteLine("Número inválido.");
        return;
}

switch (dia)
{
    case 6:
    case 7:
        Console.WriteLine(nombre + ": fin de semana. ");
        break;
    default:
        Console.WriteLine(nombre + ": dia laborable. ");
        break;
}