using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_ProyectoDAM.Models;

namespace WebService_ProyectoDAM.Servicios
{
    public class servicioReclutamientoTropas
    {
        // Metodo encargado de poner la orden de reclutamiento de tropas en caso de ser posible y devuelve un entero segun el resultado
        public int reclutarTropas(int idPueblo, int idTropa, int cantidad)
        {
            // Devolvemos error 0 --> Todo correcto; 1 --> Falta poblacion; 2 --> Error desconocido
            int error = 0;
            double tiempoTotal;

            try
            {


                using (var context = new ProyectoDAMEntities())
                {
                    // Almacenamos la informacion de la tropa necesaria para calcular si es posible el reclutamiento
                    var reclutamiento = (from register in context.Tropas
                                         where register.id_Tropas == idTropa
                                         select new
                                         {
                                             register.poblacion,
                                             register.tiempoReclutamiento
                                         }).FirstOrDefault();

                    // Almacenamos la informacion del pueblo necesaria para calcular si es posible el reclutamiento
                    var pueblo = (from register in context.Pueblo
                                  where register.id_Pueblo == idPueblo
                                  select new
                                  {
                                      register.poblacion
                                  }).FirstOrDefault();

                    // Comprobamos si hay espacio en el pueblo para reclutar la cantidad de tropas pedida por el usuario
                    if (reclutamiento.poblacion * cantidad < pueblo.poblacion)
                    {
                        // Calculamos cuanto tarda en total en realizarse el reclutamiento
                        tiempoTotal = reclutamiento.tiempoReclutamiento.Value.TotalSeconds * cantidad;

                        // Almacenamos toda la informacion necesaria en la tabla de ordenesReclutamiento
                        ordenReclutamiento insertOrden = new ordenReclutamiento();
                        insertOrden.cantidad = cantidad;
                        insertOrden.pueblo = idPueblo;
                        insertOrden.tropa = idTropa;
                        insertOrden.horaFin = DateTime.Now.AddSeconds(tiempoTotal);

                        context.ordenReclutamiento.Add(insertOrden);
                        context.SaveChanges();
                        
                    }
                    else
                    {
                        // En caso de que no haya suficiente poblacion disponible devolvemos 1
                        error = 1;
                    }
                }
            }
            catch
            {
                // En caso de que ocurra un error desconocido devolvemos 2 
                error = 2;
            }

            // Devolvemos el codigo del error
            return error; 
        }
    }
}