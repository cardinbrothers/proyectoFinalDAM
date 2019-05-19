using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_ProyectoDAM.ApiEntities;
using WebService_ProyectoDAM.Models;

namespace WebService_ProyectoDAM.Servicios
{
    public class servicioManejoMovimientos
    {
        // Metodo que realiza un nuevo movimiento
        public void realizarMovimiento(movimientossEntity movimiento)
        {
            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos la informacion del pueblo de origen
                    var puebloOrigen = (from register in context.Pueblo
                                        where register.id_Pueblo == movimiento.puebloOrigen
                                        select register).FirstOrDefault();

                    // Restamos las tropas mandadas en el movimiento del pueblo
                    puebloOrigen.arqueros -= movimiento.arqueros;
                    puebloOrigen.ballesteros -= movimiento.arqueros;
                    puebloOrigen.piqueros -= movimiento.arqueros;
                    puebloOrigen.caballeros -= movimiento.arqueros;
                    puebloOrigen.paladines -= movimiento.arqueros;

                    // Creamos un registro de movimiento con la informacion recibida
                    Movimientos insertMovimiento = new Movimientos();
                    insertMovimiento.puebloOrigen = movimiento.puebloOrigen;
                    insertMovimiento.puebloDestino = movimiento.puebloDestino;
                    insertMovimiento.duracion = movimiento.duracion;
                    insertMovimiento.horaLlegada = DateTime.Now.AddSeconds(movimiento.duracion.TotalSeconds);
                    insertMovimiento.arqueros = movimiento.arqueros;
                    insertMovimiento.ballesteros = movimiento.ballesteros;
                    insertMovimiento.piqueros = movimiento.piqueros;
                    insertMovimiento.caballeros = movimiento.caballeros;
                    insertMovimiento.paladines = movimiento.paladines;

                    // Añadimos el registro del movimiento a la tabla de movimientos y confirmamos los cambios en la base de datos
                    context.Movimientos.Add(insertMovimiento);
                    context.SaveChanges();

                }
            }
            catch
            {

            }
        }

        // Metodo que devuelve todos los movimientos enviados y recibidos actualmente por un pueblo
        public List<movimientossEntity> obtenerMovimientosActivos(int id_Pueblo)
        {
            // Creamos la lista para devolver los movientos del pueblo
            List<movimientossEntity> listaMovimientos = new List<movimientossEntity>();

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos los movientos de la base de datos
                    var movimientos = from register in context.Movimientos
                                      where (register.puebloOrigen == id_Pueblo ||
                                      register.puebloDestino == id_Pueblo) &&
                                      register.horaLlegada > DateTime.Now
                                      select register;

                    // Tratamos cada moviento con un foreach
                    foreach (var movimiento in movimientos)
                    {
                        // Creamos un objeto auxiliar para almacenar la informacion de el movimiento
                        movimientossEntity movimientoAux = new movimientossEntity();
                        movimientoAux.id_Movimiento = movimiento.id_Movimiento;
                        movimientoAux.puebloOrigen = movimiento.puebloOrigen;
                        movimientoAux.puebloDestino = movimiento.puebloDestino;
                        movimientoAux.tipoMovimiento = movimiento.tipoMovimiento;
                        movimientoAux.duracion = movimiento.duracion;
                        movimientoAux.horaLlegada = movimiento.horaLlegada;
                        movimientoAux.arqueros = (int)movimiento.arqueros;
                        movimientoAux.ballesteros = (int)movimiento.ballesteros;
                        movimientoAux.piqueros = (int)movimiento.piqueros;
                        movimientoAux.caballeros = (int)movimiento.caballeros;
                        movimientoAux.paladines = (int)movimiento.paladines;
                        movimientoAux.vencedor = (int)movimiento.vencedor;

                        // Añadimos la variable auxiliar a la lista que devolveremos
                        listaMovimientos.Add(movimientoAux);
                    }
                }
            }
            catch
            {

            }

            // Devolvemos la lista de movimientos
            return listaMovimientos;
        }
    }
}

// Metodos: RealizarApoyo; RealizarAtaque; ObtenerMovimientos; LlegadaMovimiento; Batalla; ObtenerVencedor;