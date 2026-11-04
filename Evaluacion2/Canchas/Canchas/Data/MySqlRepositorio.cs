using Canchas.Models;
using MySqlConnector;

namespace Canchas.Data;

public class MySqlRepositorio : IRepositorio
{
    // Las reservas se leen junto con su cliente y su cancha (JOIN).
    private const string SqlReservas = @"
        SELECT r.id, r.cliente_id, r.cancha_id, r.inicio, r.fin, r.estado, r.total,
               c.nombre AS cliente_nombre, c.telefono AS cliente_telefono, c.email AS cliente_email,
               ca.nombre AS cancha_nombre, ca.tipo AS cancha_tipo, ca.precio_hora, ca.activa
        FROM reservas r
        INNER JOIN clientes c ON c.id = r.cliente_id
        INNER JOIN canchas ca ON ca.id = r.cancha_id";

    private readonly string _cadena;

    public MySqlRepositorio(IConfiguration configuracion)
    {
        _cadena = configuracion.GetConnectionString("MySql") ?? "";
    }

    private async Task<MySqlConnection> Abrir()
    {
        MySqlConnection conexion = new MySqlConnection(_cadena);
        await conexion.OpenAsync();
        return conexion;
    }

    // ---------------------------------------------------------- Lecturas

    public async Task<List<Reserva>> ListarReservas()
    {
        List<Reserva> reservas = new List<Reserva>();

        await using MySqlConnection conexion = await Abrir();
        await using MySqlCommand comando = new MySqlCommand(SqlReservas + " ORDER BY r.inicio", conexion);
        await using MySqlDataReader lector = await comando.ExecuteReaderAsync();

        while (await lector.ReadAsync())
        {
            reservas.Add(LeerReserva(lector));
        }

        return reservas;
    }

    public async Task<Reserva?> ObtenerReserva(int id)
    {
        await using MySqlConnection conexion = await Abrir();
        await using MySqlCommand comando = new MySqlCommand(SqlReservas + " WHERE r.id = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);
        await using MySqlDataReader lector = await comando.ExecuteReaderAsync();

        if (await lector.ReadAsync())
            return LeerReserva(lector);

        return null;
    }

    public async Task<List<Cliente>> ListarClientes()
    {
        List<Cliente> clientes = new List<Cliente>();

        await using MySqlConnection conexion = await Abrir();
        await using MySqlCommand comando = new MySqlCommand(
            "SELECT id, nombre, telefono, email FROM clientes ORDER BY nombre", conexion);
        await using MySqlDataReader lector = await comando.ExecuteReaderAsync();

        while (await lector.ReadAsync())
        {
            clientes.Add(new Cliente
            {
                Id = Convert.ToInt32(lector["id"]),
                Nombre = Convert.ToString(lector["nombre"]) ?? "",
                Telefono = Convert.ToString(lector["telefono"]) ?? "",
                Email = lector["email"] as string
            });
        }

        return clientes;
    }

    public async Task<List<Cancha>> ListarCanchas()
    {
        List<Cancha> canchas = new List<Cancha>();

        await using MySqlConnection conexion = await Abrir();
        await using MySqlCommand comando = new MySqlCommand(
            "SELECT id, nombre, tipo, precio_hora, activa FROM canchas ORDER BY nombre", conexion);
        await using MySqlDataReader lector = await comando.ExecuteReaderAsync();

        while (await lector.ReadAsync())
        {
            canchas.Add(LeerCancha(lector));
        }

        return canchas;
    }

    public async Task<Cancha?> ObtenerCancha(int id)
    {
        await using MySqlConnection conexion = await Abrir();
        await using MySqlCommand comando = new MySqlCommand(
            "SELECT id, nombre, tipo, precio_hora, activa FROM canchas WHERE id = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);
        await using MySqlDataReader lector = await comando.ExecuteReaderAsync();

        if (await lector.ReadAsync())
            return LeerCancha(lector);

        return null;
    }

    public async Task<bool> HayConflicto(int canchaId, DateTime inicio, DateTime fin, int ignorarReservaId)
    {
        // Dos horarios se cruzan cuando uno empieza antes de que el otro termine, y viceversa.
        const string sql = @"
            SELECT COUNT(*) FROM reservas
            WHERE cancha_id = @cancha
              AND estado <> 'Cancelada'
              AND inicio < @fin
              AND fin > @inicio
              AND id <> @ignorar";

        await using MySqlConnection conexion = await Abrir();
        await using MySqlCommand comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@cancha", canchaId);
        comando.Parameters.AddWithValue("@inicio", inicio);
        comando.Parameters.AddWithValue("@fin", fin);
        comando.Parameters.AddWithValue("@ignorar", ignorarReservaId);

        object? cantidad = await comando.ExecuteScalarAsync();
        return Convert.ToInt32(cantidad) > 0;
    }

    // ---------------------------------------------------------- Escrituras

    public async Task CrearReserva(Reserva reserva)
    {
        const string sql = @"
            INSERT INTO reservas (cliente_id, cancha_id, inicio, fin, estado, total)
            VALUES (@cliente, @cancha, @inicio, @fin, @estado, @total)";

        await using MySqlConnection conexion = await Abrir();
        await using MySqlCommand comando = new MySqlCommand(sql, conexion);
        AgregarParametros(comando, reserva);
        await comando.ExecuteNonQueryAsync();
    }

    public async Task ActualizarReserva(Reserva reserva)
    {
        const string sql = @"
            UPDATE reservas
            SET cliente_id = @cliente, cancha_id = @cancha, inicio = @inicio,
                fin = @fin, estado = @estado, total = @total
            WHERE id = @id";

        await using MySqlConnection conexion = await Abrir();
        await using MySqlCommand comando = new MySqlCommand(sql, conexion);
        AgregarParametros(comando, reserva);
        comando.Parameters.AddWithValue("@id", reserva.Id);
        await comando.ExecuteNonQueryAsync();
    }

    public async Task EliminarReserva(int id)
    {
        await using MySqlConnection conexion = await Abrir();
        await using MySqlCommand comando = new MySqlCommand("DELETE FROM reservas WHERE id = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);
        await comando.ExecuteNonQueryAsync();
    }

    // ---------------------------------------------------------- Apoyo

    private static void AgregarParametros(MySqlCommand comando, Reserva reserva)
    {
        comando.Parameters.AddWithValue("@cliente", reserva.ClienteId);
        comando.Parameters.AddWithValue("@cancha", reserva.CanchaId);
        comando.Parameters.AddWithValue("@inicio", reserva.Inicio);
        comando.Parameters.AddWithValue("@fin", reserva.Fin);
        comando.Parameters.AddWithValue("@estado", reserva.Estado);
        comando.Parameters.AddWithValue("@total", reserva.Total);
    }

    private static Cancha LeerCancha(MySqlDataReader lector)
    {
        return new Cancha
        {
            Id = Convert.ToInt32(lector["id"]),
            Nombre = Convert.ToString(lector["nombre"]) ?? "",
            Tipo = Convert.ToString(lector["tipo"]) ?? "",
            PrecioHora = Convert.ToDecimal(lector["precio_hora"]),
            Activa = Convert.ToBoolean(lector["activa"])
        };
    }

    // Arma una reserva con su cliente y su cancha a partir de una fila del JOIN.
    private static Reserva LeerReserva(MySqlDataReader lector)
    {
        return new Reserva
        {
            Id = Convert.ToInt32(lector["id"]),
            ClienteId = Convert.ToInt32(lector["cliente_id"]),
            CanchaId = Convert.ToInt32(lector["cancha_id"]),
            Inicio = Convert.ToDateTime(lector["inicio"]),
            Fin = Convert.ToDateTime(lector["fin"]),
            Estado = Convert.ToString(lector["estado"]) ?? "",
            Total = Convert.ToDecimal(lector["total"]),
            Cliente = new Cliente
            {
                Id = Convert.ToInt32(lector["cliente_id"]),
                Nombre = Convert.ToString(lector["cliente_nombre"]) ?? "",
                Telefono = Convert.ToString(lector["cliente_telefono"]) ?? "",
                Email = lector["cliente_email"] as string
            },
            Cancha = new Cancha
            {
                Id = Convert.ToInt32(lector["cancha_id"]),
                Nombre = Convert.ToString(lector["cancha_nombre"]) ?? "",
                Tipo = Convert.ToString(lector["cancha_tipo"]) ?? "",
                PrecioHora = Convert.ToDecimal(lector["precio_hora"]),
                Activa = Convert.ToBoolean(lector["activa"])
            }
        };
    }
}