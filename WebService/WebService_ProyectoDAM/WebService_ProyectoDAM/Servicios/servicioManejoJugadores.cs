using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_ProyectoDAM.ApiEntities;
using WebService_ProyectoDAM.Models;


namespace WebService_ProyectoDAM.Servicios
{
    public class servicioManejoJugadores
    {
        // Metodo que devuelve la informacion de todos los jugadores
        public List<infoJugadorEntity> obtenerJugadores(int id_Partida)
        {
            // Creamos la lista de jugadores que devolveremos
            List<infoJugadorEntity> listaJugadores = new List<infoJugadorEntity>();

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos todos los jugadores de la partida
                    var jugadores = from register in context.Jugador
                                    where register.id_Partida == id_Partida
                                    select register;

                    // Tratamos cada jugador con un foreach
                    foreach (var jugador in jugadores)
                    {
                        // Metemos la informacion del jugador en un objeto auxiliar
                        infoJugadorEntity jugadorAux = new infoJugadorEntity();
                        jugadorAux.nombreUsuario = jugador.nombreUsuario;
                        jugadorAux.contraseña = jugador.contraseña;
                        jugadorAux.id_partida = jugador.id_Partida;

                        // Introducimos el objeto auxiliar en la lista
                        listaJugadores.Add(jugadorAux);

                    }
                }
            }
            catch
            {

            }

            // Devolvemos la lista
            return listaJugadores;
        }

        // Metodo que devuelve la clasificacion de todos los jugadores de una partida
        public List<potenciajugadorEntity> obtenerClasificacion(int id_Partida)
        {
            // Creamos la lista que devolveremos
            List<potenciajugadorEntity> listaClasificacion = new List<potenciajugadorEntity>();

            try
            {
                // Creamos un objeto del servicio de manejo de pueblos para calcular la potencia de un jugador
                servicioManejoPueblos objPueblos = new servicioManejoPueblos();

                // Tratamos cada jugador de la partida con un foreach
                foreach (var jugador in obtenerJugadores(id_Partida))
                {
                    // Creamos un objeto auxiliar para almacenar el nombre y la potencia del jugador
                    potenciajugadorEntity potenciaAux = new potenciajugadorEntity();
                    potenciaAux.nombreJugador = jugador.nombreUsuario;
                    potenciaAux.potenciaJugador = objPueblos.obtenerPotenciaJugador(jugador.nombreUsuario);

                    // Añadimos el objeto auxiliar a la lista
                    listaClasificacion.Add(potenciaAux);
                }

                // Ordenamos la lista por la potencia de los jugadores, las más altas primero.
                listaClasificacion.OrderByDescending(x => x.potenciaJugador);
            }
            catch
            {

            }

            // Devolvemos la lista
            return listaClasificacion;
        }

        // Metodo para borrar todos los jugadores de una partida acabada
        public void borrarJugadores(int id_Partida)
        {
            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos todos los jugadores de la partida
                    var jugadores = from register in context.Jugador
                                    where register.id_Partida == id_Partida
                                    select register;

                    // Tratamos cada jugador con un foreach
                    foreach (var jugador in jugadores)
                    {
                        // Borramos el jugador de la tabla jugadores
                        context.Jugador.Remove(jugador);
                    }

                    // Confirmamos los cambios
                    context.SaveChanges();
                }
            }
            catch
            {
                
            }
        }

    }
}
