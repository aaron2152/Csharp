int[,] matriz =
{
    { 4, 9, 2 },
    { 3, 5, 7 },
    { 8, 1, 6 }
};

int diagonal = 0;

for (int i = 0; i < 3; i++)
{
    int sumaFila = 0;
    for (int j = 0; j < 3; j++)
    {
        Console.Write(matriz[i, j] + " ");
        sumaFila += matriz[i, j];
    }
    Console.WriteLine("  -> suma de la fila: " + sumaFila);
    diagonal += matriz[i, i];
}

Console.WriteLine("Suma de la diagonal: " + diagonal);