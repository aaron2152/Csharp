using Microsoft.AspNetCore.Mvc;
using Trivia.Data;
using Trivia.Models;

namespace Trivia.Controllers;

public class PreguntasController : Controller
{
    private readonly SupabaseCliente _supabase;

    public PreguntasController(SupabaseCliente supabase)
    {
        _supabase = supabase;
    }

    public async Task<IActionResult> Index()
    {
        List<Pregunta> preguntas = await _supabase.Listar<Pregunta>("preguntas?select=*&order=id.asc");
        return View(preguntas);
    }
    public IActionResult Create()
    {
        return View(new Pregunta());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pregunta pregunta)
    {
        if (!ModelState.IsValid)
            return View(pregunta);

        await _supabase.Insertar("preguntas", pregunta);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Edit(int id)
    {
        List<Pregunta> resultado = await _supabase.Listar<Pregunta>("preguntas?select=*&id=eq." + id);
        if (resultado.Count == 0)
            return NotFound();
        return View(resultado[0]);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Pregunta pregunta)
    {
        if (!ModelState.IsValid)
            return View(pregunta);

        await _supabase.Actualizar("preguntas", pregunta.Id, pregunta);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Delete(int id)
    {
        List<Pregunta> resultado = await _supabase.Listar<Pregunta>("preguntas?select=*&id=eq." + id);
        if (resultado.Count == 0)
            return NotFound();
        return View(resultado[0]);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _supabase.Eliminar("preguntas", id);
        return RedirectToAction("Index");
    }
}