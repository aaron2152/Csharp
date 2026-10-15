using System;
double numero = 2;
int cantidadPrimos = 0;
bool esPrimo;
while (cantidadPrimos < 5)
{
    // 2.... numero - 1
    esPrimo = true;
    for (int i = 2; i <= (int) numero/2; i++) {
        if (numero % i == 0) {
            esPrimo = false;
            break;
        }
    }
    if (esPrimo) {
        Console.WriteLine($"{numero}");
        cantidadPrimos++;
    }
    numero++;
}