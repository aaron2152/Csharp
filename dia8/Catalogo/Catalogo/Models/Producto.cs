namespace Catalogo.Models
{
    public class Producto
    {
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Categoria { get; set; } = "";
        public double PrecioVenta { get; set; }
        public int Stock {  get; set; }
    }
}
