using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_ProyectoDAM.Models;
using WebService_ProyectoDAM.ApiEntities;

namespace WebService_ProyectoDAM.Servicios
{
    public class servicioReclutamientoTropas
    {
        // Metodo encargado de poner la orden de reclutamiento de tropas en caso de ser posible y devuelve la informacion de la orden de reclutamiento
        public OrdenReclutamientoEntity reclutarTropas(int idPueblo, int idTropa, int cantidad)
        {
            //Creamos el objeto que devolveremos 
            OrdenReclutamientoEntity resultado = new OrdenReclutamientoEntity();

            // Devolvemos en el error 0 --> Todo correcto; 1 --> Falta poblacion; 2 --> Error desconocido
            resultado.error = 0;

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

                        resultado.id_Tropa = idTropa;
                        resultado.cantidad = cantidad;
                        resultado.horaFin = DateTime.Now.AddSeconds(tiempoTotal);

                    }
                    else
                    {
                        // En caso de que no haya suficiente poblacion disponible devolvemos 1
                        resultado.error = 1;
                    }
                }
            }
            catch
            {
                // En caso de que ocurra un error desconocido devolvemos 2 
                resultado.error = 2;
            }

            // Devolvemos la informacion de reclutamiento
            return resultado;
        }

        // Metodo que completa una orden de reclutamiento y la añade al pueblo. 
        public int completarOrdenReclutamiento(int idOrden)
        {
            // Devolvemos error 0 --> Todo correcto; 1 --> Orden ya completada; 2 --> Error desconocido
            int error = 0;

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Almacenamos la informacion de la orden de reclutamiento que hay que completar
                    var ordenCompletada = (from register in context.ordenReclutamiento
                                           where register.idOrden == idOrden
                                           select register).FirstOrDefault();

                    // Comprobamos que efectivamente no ha sido completada hasta ahora la orden
                    if (ordenCompletada.terminado == false)
                    {
                        // Recogemos el pueblo al cual pertenece la orden
                        var pueblo = (from register in context.Pueblo
                                      where register.id_Pueblo == ordenCompletada.pueblo
                                      select register).FirstOrDefault();

                        // Incrementamos la tropa que corresponda
                        switch (ordenCompletada.tropa)
                        {
                            case 0:
                                pueblo.arqueros = pueblo.arqueros + ordenCompletada.cantidad;
                                break;
                            case 1:
                                pueblo.ballesteros = pueblo.ballesteros + ordenCompletada.cantidad;
                                break;
                            case 2:
                                pueblo.piqueros = pueblo.piqueros + ordenCompletada.cantidad;
                                break;
                            case 3:
                                pueblo.caballeros = pueblo.caballeros + ordenCompletada.cantidad;
                                break;
                            case 4:
                                pueblo.paladines = pueblo.paladines + ordenCompletada.cantidad;
                                break;
                        }

                        // Marcamos comom completada la orden de reclutamiento
                        ordenCompletada.terminado = true;

                        // Guardamos los cambios en la base de datos
                        context.SaveChanges();

                    }
                    else
                    {
                        // En caso de que ya se hubiera completado la orden devolvemos 1
                        error = 1;
                    }
                }
            }
            catch
            {
                // En caso de que ocurra un error desconocido devolvemos 2 
                error = 2;
            }

            // Devolvemos el codigo de error obtenido
            return error;
        }

        // Metodo que obtiene todas las ordenes que no hayan terminado y completa aquellas que si
        public List<OrdenReclutamientoEntity> obtenerOrdenesActivas(int idPueblo)
        {
            // Creamos la lista de ordenes que devolveremos
            List<OrdenReclutamientoEntity> listaOrdenes = new List<OrdenReclutamientoEntity>();

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Almacenamos todas las ordenes que no hayan sido completadas del pueblo recibido
                    var ordenesPueblo = from register in context.ordenReclutamiento
                                        where register.pueblo == idPueblo && register.terminado == true
                                         select register;


                    // Tratamos cada orden almacenada con un foreach
                    foreach (var orden in ordenesPueblo)
                    {
                        // Comprobamos si la hora de finalizar ya ha excedido la hora actual
                        if (orden.horaFin > DateTime.Now)
                        {
                            // Llamamos al metodo que completa la orden de reclutamiento
                            completarOrdenReclutamiento(orden.idOrden);
                        }
                        else
                        {
                            // En caso de que no se haya expirado aun la tenemos que devolver

                            OrdenReclutamientoEntity ordenAuxiliar = new OrdenReclutamientoEntity(); // Creamos un objeto auxiliar para almacenar la informacion de la orden actual

                            // Almacenamos lo necesario en el objeto auxiliar
                            ordenAuxiliar.id_Tropa = orden.tropa;
                            ordenAuxiliar.cantidad = (int)orden.cantidad;
                            ordenAuxiliar.horaFin = orden.horaFin;

                            // Añadimos el objeto auxiliar a la lista que devolveremos
                            listaOrdenes.Add(ordenAuxiliar);
                        }
                    }
                }
            }
            catch
            {

            }

            // Devolvemos la lista de ordenes
            return listaOrdenes;
        }
    }
}