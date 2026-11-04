List<string> compras = new List<string> { "pan", "huevos", "leche" };

compras.Add("Yerba");
compras.Insert(0, "Arroz");
compras.Remove("leche");
compras.Sort();

foreach (string producto in compras)
{
    Console.WriteLine("- " + producto);
}
Console.WriteLine("¿Hay huevos? " + compras.Contains("huevos"));