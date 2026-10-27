using Microsoft.AspNetCore.Mvc;
using Catalogo.Models;

namespace Catalogo.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Index()
        {
            List<Producto> productos = new List<Producto>
            {
                // Consolas
                new Producto { Codigo = "K003", Nombre = "PS5 Slim 1TB con Disco", Categoria = "Consola", PrecioVenta = 5000000, Stock = 10 },
                new Producto { Codigo = "K004", Nombre = "PS5 Pro 2TB Digital", Categoria = "Consola", PrecioVenta = 8000000, Stock = 3 },
                new Producto { Codigo = "K006", Nombre = "Xbox Series X 1TB", Categoria = "Consola", PrecioVenta = 5200000, Stock = 7 },
                new Producto { Codigo = "K008", Nombre = "Nintendo Switch OLED 64GB", Categoria = "Consola", PrecioVenta = 3200000, Stock = 12 },
                new Producto { Codigo = "K015", Nombre = "Meta Quest 3 128GB VR", Categoria = "Consola", PrecioVenta = 4800000, Stock = 5 },

                // Dispositivos y Accesorios
                new Producto { Codigo = "K005", Nombre = "Mando Dualsense PS5", Categoria = "Dispositivo", PrecioVenta = 600000, Stock = 10 },
                new Producto { Codigo = "K007", Nombre = "Control Inalámbrico Xbox Series", Categoria = "Dispositivo", PrecioVenta = 550000, Stock = 15 },
                new Producto { Codigo = "K011", Nombre = "Auriculares Inalámbricos Pulse 3D", Categoria = "Dispositivo", PrecioVenta = 900000, Stock = 6 },
                new Producto { Codigo = "K013", Nombre = "Volante Logitech G29 + Pedales", Categoria = "Dispositivo", PrecioVenta = 2500000, Stock = 4 },

                // Videojuegos
                new Producto { Codigo = "K001", Nombre = "Red Dead Redemption 2", Categoria = "Videojuego", PrecioVenta = 200000, Stock = 19 },
                new Producto { Codigo = "K002", Nombre = "Grand Theft Auto VI", Categoria = "Videojuego", PrecioVenta = 450000, Stock = 4 },
                new Producto { Codigo = "K009", Nombre = "The Legend of Zelda: Tears of the Kingdom", Categoria = "Videojuego", PrecioVenta = 450000, Stock = 8 },
                new Producto { Codigo = "K010", Nombre = "Cyberpunk 2077: Ultimate Edition", Categoria = "Videojuego", PrecioVenta = 350000, Stock = 14 },
                new Producto { Codigo = "K012", Nombre = "Elden Ring: Shadow of the Erdtree Edition", Categoria = "Videojuego", PrecioVenta = 380000, Stock = 11 },
                new Producto { Codigo = "K014", Nombre = "Minecraft Standard Edition", Categoria = "Videojuego", PrecioVenta = 200000, Stock = 25 }
            };

            // Opcional: Ordenar mediante LINQ asegurando que se mantenga el orden por categoría
            var productosOrdenados = productos.OrderBy(p => p.Categoria).ToList();

            return View(productosOrdenados);
        }
    }
}