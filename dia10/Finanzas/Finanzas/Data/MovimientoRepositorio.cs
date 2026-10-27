using MySqlConnector;
using Finanzas.Models;

namespace Finanzas.Data;

public class MovimientoRepositorio
{
    private readonly string _cadenaConexion;

    public MovimientoRepositorio(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    public List<Movimiento> ObtenerTodos()
    {
        List<Movimiento> movimientos = new List<Movimiento>();
        using MySqlConnection conexion = new MySqlConnection(_cadenaConexion);
        conexion.Open();
        using MySqlCommand comando = new MySqlCommand("SELECT id, monto, tipo, descripcion FROM movimientos ORDER BY id DESC", conexion);
        using MySqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            movimientos.Add(LeerMovimiento(lector));
        }
        return movimientos;
    }

    public Movimiento? ObtenerPorId(int id)
    {
        using MySqlConnection conexion = new MySqlConnection(_cadenaConexion);
        conexion.Open();
        using MySqlCommand comando = new MySqlCommand("SELECT id, monto, tipo, descripcion FROM movimientos WHERE id = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);
        using MySqlDataReader lector = comando.ExecuteReader();
        if (lector.Read()) return LeerMovimiento(lector);
        return null;
    }

    public void Agregar(Movimiento movimiento)
    {
        using MySqlConnection conexion = new MySqlConnection(_cadenaConexion);
        conexion.Open();
        using MySqlCommand comando = new MySqlCommand("INSERT INTO movimientos (monto, tipo, descripcion) VALUES (@monto, @tipo, @descripcion)", conexion);
        comando.Parameters.AddWithValue("@monto", movimiento.Monto);
        comando.Parameters.AddWithValue("@tipo", movimiento.Tipo.ToString());
        comando.Parameters.AddWithValue("@descripcion", movimiento.Descripción);
        comando.ExecuteNonQuery();
    }

    public void Actualizar(Movimiento movimiento)
    {
        using MySqlConnection conexion = new MySqlConnection(_cadenaConexion);
        conexion.Open();
        using MySqlCommand comando = new MySqlCommand("UPDATE movimientos SET monto = @monto, tipo = @tipo, descripcion = @descripcion WHERE id = @id", conexion);
        comando.Parameters.AddWithValue("@monto", movimiento.Monto);
        comando.Parameters.AddWithValue("@tipo", movimiento.Tipo.ToString());
        comando.Parameters.AddWithValue("@descripcion", movimiento.Descripción);
        comando.Parameters.AddWithValue("@id", movimiento.Id);
        comando.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using MySqlConnection conexion = new MySqlConnection(_cadenaConexion);
        conexion.Open();
        using MySqlCommand comando = new MySqlCommand("DELETE FROM movimientos WHERE id = @id", conexion);
        comando.Parameters.AddWithValue("@id", id);
        comando.ExecuteNonQuery();
    }

    private Movimiento LeerMovimiento(MySqlDataReader lector)
    {
        Movimiento movimiento = new Movimiento();
        movimiento.Id = lector.GetInt32(0);
        movimiento.Monto = lector.GetInt32(1);
        movimiento.Tipo = Enum.Parse<TipoMovimiento>(lector.GetString(2));
        movimiento.Descripción = lector.GetString(3);
        return movimiento;
    }
}