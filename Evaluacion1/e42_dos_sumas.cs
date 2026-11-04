int[] numeros = { 2, 7, 11, 15, 3, 6 };
int objetivo = 9;

Dictionary<int, int> vistos = new Dictionary<int, int>();

for (int i = 0; i < numeros.Length; i++)
{
    int falta = objetivo - numeros[i];
    if (vistos.ContainsKey(falta))
    {
        Console.WriteLine("Posiciones " + vistos[falta] + " y " + i + ": " + falta + " + " + numeros[i] + " = " + objetivo);
        return;
    }

    vistos[numeros[i]] = i;
}

Console.WriteLine("No hay dos números que sumen " + objetivo + ".");