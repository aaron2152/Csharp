
using PiedraPapelTijera.Models;

namespace PiedraPapelTijera.Services;

public class JuegoServicio
{
    // Crear una nueva partida
    public Partida IniciarPartida(string nombreJugador)
    {
        if (string.IsNullOrWhiteSpace(nombreJugador))
            throw new ArgumentException("Debes ingresar un nombre.");

        nombreJugador = nombreJugador.Trim();

        if (nombreJugador.Length > 20)
            throw new ArgumentException("Máximo 20 caracteres.");

        return new Partida
        {
            NombreJugador = nombreJugador,
            Fichas = 100,
            RondasJugadas = 0,
            Finalizada = false,
            Estado = "En curso"
        };
    }

    // Procesar una ronda
    public ResultadoRonda JugarRonda(
        Partida partida,
        string jugadaJugador,
        int apuesta)
    {
        if (partida == null)
            throw new ArgumentException("La partida no existe.");

        if (partida.Finalizada)
            throw new InvalidOperationException(
                "La partida ya ha terminado.");

        // Validar la apuesta
        if (apuesta < 1 || apuesta > partida.Fichas)
        {
            throw new ArgumentException(
                "La apuesta debe estar entre 1 y tus fichas disponibles.");
        }

        // Obtener la jugada del jugador
        Jugada jugador = CrearJugada(jugadaJugador);

        // La computadora elige en el servidor
        Jugada computadora = CrearJugadaAleatoria();

        // Determinar el resultado
        string resultado;

        if (jugador.GanaA(computadora))
        {
            partida.Fichas += apuesta;
            resultado = "Victoria";
        }
        else if (computadora.GanaA(jugador))
        {
            partida.Fichas -= apuesta;
            resultado = "Derrota";
        }
        else
        {
            resultado = "Empate";
        }

        partida.RondasJugadas++;

        // Comprobar si terminó la partida
        if (partida.Fichas >= 500)
        {
            partida.Finalizada = true;
            partida.Estado = "Ganada";
        }
        else if (partida.Fichas <= 0)
        {
            partida.Finalizada = true;
            partida.Estado = "Perdida";
        }

        return new ResultadoRonda
        {
            JugadaJugador = jugador.Nombre,
            JugadaComputadora = computadora.Nombre,
            Apuesta = apuesta,
            Resultado = resultado,
            FichasFinales = partida.Fichas
        };
    }

    // Convertir el texto recibido en una jugada válida
    private Jugada CrearJugada(string nombre)
    {
        return nombre?.Trim().ToLowerInvariant() switch
        {
            "piedra" => new Piedra(),
            "papel" => new Papel(),
            "tijera" => new Tijera(),

            _ => throw new ArgumentException(
                "La jugada seleccionada no es válida.")
        };
    }

    // Generar jugada aleatoria de la computadora
    private Jugada CrearJugadaAleatoria()
    {
        int numero = Random.Shared.Next(3);

        return numero switch
        {
            0 => new Piedra(),
            1 => new Papel(),
            2 => new Tijera(),

            _ => throw new InvalidOperationException()
        };
    }
}
