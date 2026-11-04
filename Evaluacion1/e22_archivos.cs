string ruta = "nombres.txt";
string[] nombres = { "Lucía", "Mateo", "Rosa", "Pedro" };

File.WriteAllLines(ruta, nombres);
File.AppendAllText(ruta, "Ana" + Environment.NewLine);

string[] lineas = File.ReadAllLines(ruta);
for (int i = 0; i < lineas.Length; i++)
{
    Console.WriteLine((i + 1) + ". " + lineas[i]);
}

Console.WriteLine("Total: " + lineas.Length + " nombres.");

File.Delete(ruta);