string frase = "el gato y el perro y el loro";
string[] palabras = frase.Split(' ');

Dictionary<string, int> conteo = new Dictionary<string, int>();

foreach (string palabra in palabras)
{
    if (conteo.ContainsKey(palabra))
        conteo[palabra]++;
    else
        conteo[palabra] = 1;
}

foreach (KeyValuePair<string, int> par in conteo)
{
    Console.WriteLine(par.Key + ": " + par.Value);
}

string masFrecuente = "";
int maximo = 0;

foreach (KeyValuePair<string, int> par in conteo)
{
    if (par.Value > maximo)
    {
        maximo = par.Value;
        masFrecuente = par.Key;
    }
}

Console.WriteLine("Más repetida: " + masFrecuente + " (" + maximo + " veces)");