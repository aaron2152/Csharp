using Microsoft.AspNetCore.Mvc;
using Billetaje.Models;

namespace Billetaje.Controllers;

public class TarjetasController : Controller
{
    public IActionResult Index()
    {
        List<Tarjeta> tarjetas = new List<Tarjeta>
        {
            new Tarjeta { Numero = "0001", Titular = "Lucía Benítez", Saldo = 13200 },
            new Tarjeta { Numero = "0002", Titular = "Mateo Ayala", Saldo = 8300 },
            new Tarjeta { Numero = "0003", Titular = "Rosa Giménez", Saldo = 25000 },
            new Tarjeta { Numero = "0004", Titular = "Carlos Duarte", Saldo = 1700 }
        };

        return View(tarjetas);
    }
}