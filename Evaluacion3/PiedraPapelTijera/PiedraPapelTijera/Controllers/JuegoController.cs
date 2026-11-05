
using Microsoft.AspNetCore.Mvc;
using PiedraPapelTijera.Models;
using PiedraPapelTijera.Services;
using System.Text.Json;

namespace PiedraPapelTijera.Controllers;

public class JuegoController : Controller
{
    private readonly JuegoServicio _juego;
    private readonly RankingServicio _ranking;

    private const string ClavePartida = "Partida";
    private const string ClaveResultado = "Resultado";
    private const string ClavePuntajeGuardado = "PuntajeGuardado";

    private static readonly JsonSerializerOptions OpcionesJson = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public JuegoController(
        JuegoServicio juego,
        RankingServicio ranking)
    {
        _juego = juego;
        _ranking = ranking;
    }

    // Pantalla de inicio
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // Crear una partida
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Iniciar(string jugador)
    {
        try
        {
            Partida partida = _juego.IniciarPartida(jugador);

            GuardarPartida(partida);

            HttpContext.Session.Remove(ClaveResultado);
            HttpContext.Session.Remove(ClavePuntajeGuardado);

            return RedirectToAction("Ronda");
        }
        catch (ArgumentException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    // Mostrar la pantalla de ronda
    [HttpGet]
    public IActionResult Ronda()
    {
        Partida? partida = ObtenerPartida();

        if (partida == null || partida.Finalizada)
            return RedirectToAction("Index");

        return View(partida);
    }

    // Procesar la jugada y la apuesta
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Jugar(
        string jugada,
        int apuesta)
    {
        Partida? partida = ObtenerPartida();

        if (partida == null || partida.Finalizada)
            return RedirectToAction("Index");

        try
        {
            ResultadoRonda resultado = _juego.JugarRonda(
                partida,
                jugada,
                apuesta
            );

            // Actualizar el estado en el servidor
            GuardarPartida(partida);

            // Guardar el resultado de la ronda
            string jsonResultado = JsonSerializer.Serialize(resultado);

            HttpContext.Session.SetString(
                ClaveResultado,
                jsonResultado
            );

            // Si terminó la partida, registrar el puntaje
            if (partida.Finalizada)
            {
                try
                {
                    await GuardarPuntajeFinal(partida);
                }
                catch (Exception)
                {
                    TempData["ErrorRanking"] =
                        "No se pudo guardar el puntaje. Puedes intentarlo nuevamente.";
                }

                return RedirectToAction("Final");
            }

            return RedirectToAction("Resultado");
        }
        catch (ArgumentException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Ronda");
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Ronda");
        }
    }

    // Mostrar el resultado de una ronda
    [HttpGet]
    public IActionResult Resultado()
    {
        ResultadoRonda? resultado = ObtenerResultado();

        if (resultado == null)
            return RedirectToAction("Index");

        return View(resultado);
    }

    // Pantalla final
    [HttpGet]
    public IActionResult Final()
    {
        Partida? partida = ObtenerPartida();

        if (partida == null || !partida.Finalizada)
            return RedirectToAction("Index");

        ViewBag.UltimoResultado = ObtenerResultado();

        return View(partida);
    }

    // Volver a intentar guardar el puntaje
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReintentarGuardar()
    {
        Partida? partida = ObtenerPartida();

        if (partida == null || !partida.Finalizada)
            return RedirectToAction("Index");

        try
        {
            await GuardarPuntajeFinal(partida);
            TempData["Mensaje"] = "Puntaje guardado correctamente.";
        }
        catch (Exception)
        {
            TempData["ErrorRanking"] =
                "No se pudo guardar el puntaje. Inténtalo nuevamente.";
        }

        return RedirectToAction("Final");
    }

    // Guardar la partida en la sesión del servidor
    private void GuardarPartida(Partida partida)
    {
        string json = JsonSerializer.Serialize(partida);

        HttpContext.Session.SetString(ClavePartida, json);
    }

    // Recuperar la partida
    private Partida? ObtenerPartida()
    {
        string? json = HttpContext.Session.GetString(ClavePartida);

        if (string.IsNullOrEmpty(json))
            return null;

        return JsonSerializer.Deserialize<Partida>(
            json,
            OpcionesJson
        );
    }

    // Recuperar el resultado de la ronda
    private ResultadoRonda? ObtenerResultado()
    {
        string? json = HttpContext.Session.GetString(ClaveResultado);

        if (string.IsNullOrEmpty(json))
            return null;

        return JsonSerializer.Deserialize<ResultadoRonda>(
            json,
            OpcionesJson
        );
    }

    // Registrar el puntaje final una sola vez
    private async Task GuardarPuntajeFinal(Partida partida)
    {
        if (HttpContext.Session.GetString(ClavePuntajeGuardado) == "true")
            return;

        await _ranking.GuardarPuntaje(
            partida.NombreJugador,
            partida.Fichas
        );

        HttpContext.Session.SetString(
            ClavePuntajeGuardado,
            "true"
        );
    }
}
