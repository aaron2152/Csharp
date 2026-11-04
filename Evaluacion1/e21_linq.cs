List<Producto> productos = new List<Producto>
{
    new Producto { Nombre = "Mouse", Precio = 45000 },
    new Producto { Nombre = "Teclado", Precio = 120000 },
    new Producto { Nombre = "Monitor", Precio = 850000 },
    new Producto { Nombre = "Cable HDMI", Precio = 25000 },
    new Producto { Nombre = "Auriculares", Precio = 180000 }
};

var caros = productos.Where(p => p.Precio > 100000).OrderBy(p => p.Precio);

Console.WriteLine("Productos de más de 100.000:");
foreach (Producto p in caros)
{
    Console.WriteLine("- " + p.Nombre + ": " + p.Precio);
}

Console.WriteLine("Total: " + productos.Sum(p => p.Precio));
Console.WriteLine("Promedio: " + productos.Average(p => p.Precio));
Console.WriteLine("Más caro: " + productos.MaxBy(p => p.Precio)!.Nombre);
Console.WriteLine("Nombres: " + string.Join(", ", productos.Select(p => p.Nombre)));

class Producto
{
    public string Nombre { get; set; } = "";
    public int Precio { get; set; }
}