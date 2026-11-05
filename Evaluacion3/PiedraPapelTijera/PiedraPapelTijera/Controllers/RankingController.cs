using Microsoft.AspNetCore.Mvc;
using PiedraPapelTijera.Services;
using PiedraPapelTijera.Models;
namespace PiedraPapelTijera.Controllers;

public class RankingController : Controller
{
    private readonly RankingServicio _ranking;

    public RankingController(RankingServicio ranking)
    {
        _ranking = ranking;
    }

    // Mostrar los mejores jugadores
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var puntajes = await _ranking.ListarMejores();

            return View(puntajes);
        }
        catch (Exception)
        {
            ViewBag.Error = "No se pudo cargar el ranking. Inténtalo nuevamente.";

            return View(new List<Puntaje>());
        }
    }
}
