using System;

Dictionary<string, double> precios = new Dictionary<string, double>
{
    { "Medialuna", 2500 },
    { "Chipa", 3000 },
    { "Pan por kilo", 7000 },
    { "Gaseosa", 8000 },
    { "Leche", 9500 },
    { "Yerba", 15000 }, // Producto exento agregado
};

Dictionary<string, int> tasasIva = new Dictionary<string, int>
{
    { "Medialuna", 10 },
    { "Chipa", 10 },
    { "Pan por kilo", 5 },
    { "Gaseosa", 10 },
    { "Leche", 5 },
    { "Yerba", 0 }, // Tasa de IVA 0% (Exento)
};

Console.WriteLine("=== Catálogo ===");
foreach (string producto in precios.Keys)
{
    Console.WriteLine(producto + ": " + FormatoGs(precios[producto]) + " (IVA " + tasasIva[producto] + " %)");
}

string FormatoGs(double valor)
{
    return valor.ToString("N0") + " Gs";
}

// Carrito
List<string> carrito = new List<string>();
List<int> unidades = new List<int>();
string respuesta = "s";

while (respuesta == "s")
{
    Console.Write("Producto: ");
    string nombre = Console.ReadLine()!;

    if (!precios.ContainsKey(nombre))
    {
        Console.WriteLine("Ese producto no está en el catálogo.");
    }
    else
    {
        Console.Write("Cantidad: ");
        int cantidadPedida = int.Parse(Console.ReadLine()!);
        carrito.Add(nombre);
        unidades.Add(cantidadPedida);
    }

    Console.Write("¿Agregar otro producto? (s/n): ");
    respuesta = Console.ReadLine()!;
}

// 3. Quitar el último producto si el usuario lo desea
if (carrito.Count > 0)
{
    Console.Write($"¿Deseas quitar el último producto agregado ({carrito[carrito.Count - 1]})? (s/n): ");
    string quitar = Console.ReadLine()!;
    if (quitar.ToLower() == "s")
    {
        int ultimoIndice = carrito.Count - 1;
        Console.WriteLine($"-> Se ha eliminado: {unidades[ultimoIndice]} x {carrito[ultimoIndice]}");

        // RemoveAt en listas paralelas
        carrito.RemoveAt(ultimoIndice);
        unidades.RemoveAt(ultimoIndice);
    }
}

// Cálculo previo del Subtotal Bruto (necesario para el descuento automático)
double totalBruto = 0;
for (int i = 0; i < carrito.Count; i++)
{
    totalBruto += precios[carrito[i]] * unidades[i];
}

// 1. Descuento automático
Console.Write("Descuento en porcentaje (0 para evaluar automático): ");
double descuento = double.Parse(Console.ReadLine()!);

if (descuento == 0)
{
    descuento = CalcularDescuentoAutomatico(totalBruto);
    if (descuento > 0)
    {
        Console.WriteLine($"¡Se aplicó un descuento automático del {descuento}% por superar 100.000 Gs!");
    }
}

double totalNeto = 0;
double iva10 = 0;
double iva5 = 0;

Console.WriteLine();
Console.WriteLine("=== FACTURA ===");
for (int i = 0; i < carrito.Count; i++)
{
    string nombreItem = carrito[i];

    double bruto = precios[nombreItem] * unidades[i];
    double neto = bruto - (bruto * (descuento / 100));

    totalNeto += neto;

    // 2. Corrección del IVA: ignorar productos con tasa 0 (exentos)
    if (tasasIva[nombreItem] == 10)
    {
        iva10 += neto - (neto / 1.10);
    }
    else if (tasasIva[nombreItem] == 5)
    {
        iva5 += neto - (neto / 1.05);
    }
    // Si es 0, no suma a ningún IVA

    Console.WriteLine(unidades[i] + " x " + nombreItem + ": " + FormatoGs(bruto));
}

Console.WriteLine("Subtotal: " + FormatoGs(totalBruto));
Console.WriteLine("Descuento (" + descuento + " %): -" + FormatoGs(totalBruto - totalNeto));
Console.WriteLine("TOTAL A PAGAR: " + FormatoGs(totalNeto));
Console.WriteLine("IVA 10 % incluido: " + FormatoGs(iva10));
Console.WriteLine("IVA 5 % incluido: " + FormatoGs(iva5));
Console.WriteLine("IVA 0 % (Exento): 0 Gs");

// Método para calcular el descuento automático
double CalcularDescuentoAutomatico(double subtotal)
{
    if (subtotal > 100000)
    {
        return 5;
    }
    return 0;
}