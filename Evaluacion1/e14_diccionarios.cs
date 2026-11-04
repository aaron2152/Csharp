Dictionary<string, int> stock = new Dictionary<string, int>
{
    { "manzana", 20 },
    { "banana", 35 },
    { "naranja", 12 },
};

stock["manzana"] -= 3;
stock["pera"] = 8;

foreach (KeyValuePair<string, int> item in stock)
{
    Console.WriteLine(item.Key + ": " + item.Value);
}

if (stock.TryGetValue("uva", out int cantidad))
    Console.WriteLine("Uvas: " + cantidad);
else
    Console.WriteLine("No hay uvas en el inventario.");