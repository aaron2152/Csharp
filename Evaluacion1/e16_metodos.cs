decimal precio = 150000m;

decimal CalcularIva(decimal monto, decimal porcentaje)
{
    return monto * porcentaje / 100;
}

decimal PrecioConIva(decimal monto, decimal porcentaje)
{
    return monto + CalcularIva(monto, porcentaje);
}

Console.WriteLine("Precio sin IVA: " + precio);
Console.WriteLine("IVA 10%: " + CalcularIva(precio, 10));
Console.WriteLine("Total con IVA 10%: " + PrecioConIva(precio, 10));
Console.WriteLine("Total con IVA 5%: " + PrecioConIva(precio, 5));