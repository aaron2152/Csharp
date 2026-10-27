using Microsoft.AspNetCore.Mvc;
using Trivia.Data;
using Trivia.Models;

namespace Trivia.Controllers;

public class JuegoController : Controller
{
    private readonly SupabaseCliente _supabase;

    public JuegoController(SupabaseCliente supabase)
    {
        _supabase = supabase;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Jugar(string jugador)
    {
        if (string.IsNullOrWhiteSpace(jugador))
            return RedirectToAction("Index");

        List<Pregunta> todas = await _supabase.Listar<Pregunta>("preguntas?select=*");
        List<Pregunta> elegidas = todas.OrderBy(p => Random.Shared.Next()).Take(5).ToList();

        ViewBag.Jugador = jugador.Trim();
        return View(elegidas);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Responder(string jugador, Dictionary<int, string> respuestas)
    {
        if (string.IsNullOrWhiteSpace(jugador) || respuestas.Count == 0)
            return RedirectToAction("Index");

        string ids = string.Join(",", respuestas.Keys);
        List<Pregunta> preguntas = await _supabase.Listar<Pregunta>("preguntas?select=*&id=in.(" + ids + ")");

        int aciertos = 0;
        foreach (Pregunta p in preguntas)
        {
            if (respuestas[p.Id] == p.Correcta)
                aciertos++;
        }

        Puntaje puntaje = new Puntaje
        {
            Jugador = jugador.Trim(),
            Puntos = aciertos * 10
        };

        await _supabase.Insertar("puntajes", puntaje);
        return RedirectToAction("Resultado", new { jugador = puntaje.Jugador, aciertos, total = respuestas.Count });
    }

    public IActionResult Resultado(string jugador, int aciertos, int total)
    {
        ViewBag.Jugador = jugador;
        ViewBag.Aciertos = aciertos;
        ViewBag.Total = total;
        return View();
    }

    public async Task<IActionResult> Ranking()
    {
        List<Puntaje> ranking = await _supabase.Listar<Puntaje>("puntajes?select=*&order=puntos.desc,creado.asc&limit=10");
        return View(ranking);
    }
}