using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_ProyectoDAM.Models;

namespace WebService_ProyectoDAM.Servicios
{
    public class servicioReclutamientoTropas
    {
        public int reclutarTropas(int idPueblo, int idTropa, int cantidad)
        {
            // Devolvemos error 0 --> Todo correcto; 1 --> Falta poblacion; 2 --> Error desconocido
            int error = 0;
            double tiempoTotal;

            try
            {


                using (var context = new ProyectoDAMEntities())
                {
                    var reclutamiento = (from register in context.Tropas
                                         where register.id_Tropas == idTropa
                                         select new
                                         {
                                             register.poblacion,
                                             register.tiempoReclutamiento
                                         }).FirstOrDefault();

                    var pueblo = (from register in context.Pueblo
                                  where register.id_Pueblo == idPueblo
                                  select new
                                  {
                                      register.poblacion
                                  }).FirstOrDefault();

                    if (reclutamiento.poblacion * cantidad < pueblo.poblacion)
                    {
                        tiempoTotal = reclutamiento.tiempoReclutamiento.Value.TotalSeconds * cantidad;
                    }
                    else
                    {
                        error = 1;
                    }
                }
            }
            catch
            {
                error = 2;
            }

            return error; // Hay que devolver el tiempo total tambien
        }
    }
}