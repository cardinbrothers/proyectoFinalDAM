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
    }
}

// Metodos: RealizarApoyo; RealizarAtaque; ObtenerMovimientos; LlegadaMovimiento; Batalla; ObtenerVencedor;