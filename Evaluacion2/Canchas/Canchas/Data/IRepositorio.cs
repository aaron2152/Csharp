using Canchas.Models;

namespace Canchas.Data;

public interface IRepositorio
{
    Task<List<Reserva>> ListarReservas();
    Task<Reserva?> ObtenerReserva(int id);
    Task<List<Cliente>> ListarClientes();
    Task<List<Cancha>> ListarCanchas();
    Task<Cancha?> ObtenerCancha(int id);

    // ¿Esa cancha ya tiene una reserva confirmada que se cruza con este horario?
    // ignorarReservaId sirve al editar, para no compararla consigo misma (0 = ninguna).
    Task<bool> HayConflicto(int canchaId, DateTime inicio, DateTime fin, int ignorarReservaId);

    Task CrearReserva(Reserva reserva);
    Task ActualizarReserva(Reserva reserva);
    Task EliminarReserva(int id);
}