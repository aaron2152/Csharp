using Canchas.Data;
using Canchas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Canchas.Controllers;

public class ReservasController : Controller
{
    private readonly IRepositorio _repositorio;

    public ReservasController(IRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    // ---------------------------------------------------------- Listar

    public async Task<IActionResult> Index()
    {
        List<Reserva> reservas = await _repositorio.ListarReservas();
        List<Cancha> canchas = await _repositorio.ListarCanchas();

        List<Reserva> confirmadas = reservas.Where(r => r.Estado == "Confirmada").ToList();

        // Datos del resumen (dashboard) que se muestran arriba de la lista.
        ViewBag.Confirmadas = confirmadas.Count;
        ViewBag.ReservasHoy = confirmadas.Count(r => r.Inicio.Date == Hoy());
        ViewBag.Ingresos = confirmadas.Sum(r => r.Total);
        ViewBag.CanchasActivas = canchas.Count(c => c.Activa);

        return View(reservas);
    }

    // ---------------------------------------------------------- Crear

    public async Task<IActionResult> Create()
    {
        await CargarListas();

        DateTime manana = Hoy().AddDays(1);
        return View(new Reserva { Inicio = manana.AddHours(18), Fin = manana.AddHours(19) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ClienteId,CanchaId,Inicio,Fin")] Reserva reserva)
    {
        if (reserva.Inicio < AhoraEnParaguay())
            ModelState.AddModelError("Inicio", "No puedes reservar en el pasado.");

        Cancha? cancha = await ValidarReserva(reserva, 0);

        if (!ModelState.IsValid || cancha == null)
        {
            await CargarListas();
            return View(reserva);
        }

        reserva.Estado = "Confirmada";
        reserva.Total = CalcularTotal(reserva, cancha);
        await _repositorio.CrearReserva(reserva);

        TempData["Mensaje"] = "Reserva creada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // ---------------------------------------------------------- Editar

    public async Task<IActionResult> Edit(int id)
    {
        Reserva? reserva = await _repositorio.ObtenerReserva(id);
        if (reserva == null)
            return NotFound();

        await CargarListas();
        return View(reserva);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,CanchaId,Inicio,Fin,Estado")] Reserva reserva)
    {
        if (id != reserva.Id || await _repositorio.ObtenerReserva(id) == null)
            return NotFound();

        if (reserva.Estado != "Confirmada" && reserva.Estado != "Cancelada")
            ModelState.AddModelError("Estado", "El estado no es válido.");

        Cancha? cancha = await ValidarReserva(reserva, reserva.Id);

        if (!ModelState.IsValid || cancha == null)
        {
            await CargarListas();
            return View(reserva);
        }

        reserva.Total = CalcularTotal(reserva, cancha);
        await _repositorio.ActualizarReserva(reserva);

        TempData["Mensaje"] = "Reserva actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // ---------------------------------------------------------- Eliminar

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _repositorio.EliminarReserva(id);

        TempData["Mensaje"] = "Reserva eliminada.";
        return RedirectToAction(nameof(Index));
    }

    // ---------------------------------------------------------- Apoyo

    // Revisa las reglas de una reserva y devuelve la cancha elegida (o null si no existe).
    private async Task<Cancha?> ValidarReserva(Reserva reserva, int ignorarId)
    {
        List<Cliente> clientes = await _repositorio.ListarClientes();
        if (!clientes.Any(c => c.Id == reserva.ClienteId))
            ModelState.AddModelError("ClienteId", "Elige un cliente.");

        Cancha? cancha = await _repositorio.ObtenerCancha(reserva.CanchaId);
        if (cancha == null)
        {
            ModelState.AddModelError("CanchaId", "Elige una cancha.");
            return null;
        }

        if (reserva.Fin <= reserva.Inicio)
            ModelState.AddModelError("Fin", "La hora de fin debe ser posterior al inicio.");
        else if ((reserva.Fin - reserva.Inicio).TotalMinutes % 60 != 0)
            ModelState.AddModelError("Fin", "La reserva debe durar horas completas (1 h, 2 h...).");

        // Una reserva cancelada no ocupa la cancha, así que solo se revisan las confirmadas.
        if (reserva.Estado == "Confirmada")
        {
            if (!cancha.Activa)
                ModelState.AddModelError("CanchaId", "Esa cancha está en mantenimiento.");

            if (ModelState.IsValid && await _repositorio.HayConflicto(cancha.Id, reserva.Inicio, reserva.Fin, ignorarId))
                ModelState.AddModelError("", "Esa cancha ya está reservada en ese horario.");
        }

        return cancha;
    }

    private static decimal CalcularTotal(Reserva reserva, Cancha cancha)
    {
        decimal horas = (decimal)(reserva.Fin - reserva.Inicio).TotalHours;
        return horas * cancha.PrecioHora;
    }

    // Llena las listas desplegables del formulario.
    private async Task CargarListas()
    {
        List<Cliente> clientes = await _repositorio.ListarClientes();
        List<Cancha> canchas = await _repositorio.ListarCanchas();

        ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre");
        ViewBag.Canchas = new SelectList(
            canchas.Select(c => new
            {
                c.Id,
                Texto = c.Nombre + " (" + c.Tipo + ") - " + c.PrecioHora.ToString("N0") + " Gs./h" + (c.Activa ? "" : " - en mantenimiento")
            }),
            "Id", "Texto");
    }

    // Paraguay está en UTC-3 todo el año: así la fecha no depende de la zona horaria del servidor.
    private static DateTime AhoraEnParaguay()
    {
        return DateTime.UtcNow.AddHours(-3);
    }

    private static DateTime Hoy()
    {
        return AhoraEnParaguay().Date;
    }
}