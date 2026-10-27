using Microsoft.AspNetCore.Mvc;
using Agenda.Data;
using Agenda.Models;

namespace Agenda.Controllers;

public class ClientesController : Controller
{
    private readonly IConfiguration _configuracion;

    public ClientesController(IConfiguration configuracion)
    {
        _configuracion = configuracion;
    }

    public IActionResult Index()
    {
        string cadena = _configuracion.GetConnectionString("Mysql") ?? "";
        ClienteRepositorio repositorio = new ClienteRepositorio(cadena);
        List<Cliente> clientes = new List<Cliente>();

        try
        {
            clientes = repositorio.ObtenerTodos();
        }
        catch (Exception ex)
        {
            ViewBag.Error = "No se pudo leer la base de datos: " + ex.Message;
        }

        return View(clientes);
    }
}