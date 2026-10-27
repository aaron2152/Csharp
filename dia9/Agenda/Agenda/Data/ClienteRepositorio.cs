using MySqlConnector;
using Agenda.Models;

namespace Agenda.Data;

public class ClienteRepositorio
{
    private readonly string _cadenaConexion;

    public ClienteRepositorio(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    public List<Cliente> ObtenerTodos()
    {
        List<Cliente> clientes = new List<Cliente>();

        using MySqlConnection conexion = new MySqlConnection(_cadenaConexion);
        conexion.Open();

        using MySqlCommand comando = new MySqlCommand("SELECT id, nombre, telefono, ciudad FROM clientes ORDER BY nombre", conexion);
        using MySqlDataReader lector = comando.ExecuteReader();

        while (lector.Read())
        {
            Cliente cliente = new Cliente();
            cliente.Id = lector.GetInt32(0);
            cliente.Nombre = lector.GetString(1);
            cliente.Telefono = lector.GetString(2);
            cliente.Ciudad = lector.GetString(3);
            clientes.Add(cliente);
        }

        return clientes;
    }
}