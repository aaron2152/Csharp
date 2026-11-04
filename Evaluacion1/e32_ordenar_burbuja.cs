int[] numeros = { 64, 34, 25, 12, 22, 11, 90 };

Console.WriteLine("Antes:   " + string.Join(" ", numeros));

for (int pasada = 0; pasada < numeros.Length - 1; pasada++)
{
    for (int i = 0; i < numeros.Length - 1 - pasada; i++)
    {
        if (numeros[i] > numeros[i + 1])
        {
            int temporal = numeros[i];
            numeros[i] = numeros[i + 1];
            numeros[i + 1] = temporal;
        }
    }
}

Console.WriteLine("Después: " + string.Join(" ", numeros));