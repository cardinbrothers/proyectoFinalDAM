using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_ProyectoDAM.ApiEntities;
using WebService_ProyectoDAM.Models;

namespace WebService_ProyectoDAM.Servicios
{
    public class servicioMenuPrincipal
    {
        // Metodos: Crear Partida, Obtener Partida, Crear Puenta, Iniciar sesion

        public int crearPartida(infoPartidaEntity record)
        {
            int error = 0;

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    Partida partidaCreada = new Partida();

                    partidaCreada.Modalidad = record.modalidad;
                    partidaCreada.Velocidad = record.velocidad;
                    partidaCreada.Duracion = record.duracion;
                    partidaCreada.limiteJugadores = record.limiteJugadores;
                    partidaCreada.limitePoblacion = record.limitePoblacion;
                    partidaCreada.fechaInicio = DateTime.Now;
                    partidaCreada.activo = true;

                    // Borramos la orden indicada
                    context.Partida.Add(partidaCreada);
                    context.SaveChanges();
                }
            }
            catch
            {
                error = 1;
            }

            return error;
        }
    }
}