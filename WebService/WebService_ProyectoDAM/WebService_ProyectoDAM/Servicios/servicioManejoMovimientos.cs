using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        // Metodo que comprueba que movimentos han ocurrido ya y llama a los metodos que realicen las acciones correspondientes
        public void comprobarMovimientosAcabados()
        {
            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos la lista de movimientos terminados
                    var movimentosAcabados = from register in context.Movimientos
                                             where register.horaLlegada < DateTime.Now &&
                                             register.vencedor == -1
                                             select register;

                    // Tratamos cada moviento con uin foreach
                    foreach (var movimiento in movimentosAcabados)
                    {
                        // Comprobamos si el movimiento era un apoyo
                        if (movimiento.tipoMovimiento == "apoyo")
                        {
                            // Creamos un objeto para usar los metodos del servicio de apoyos
                            servicioManejoApoyos apoyos = new servicioManejoApoyos();

                            // Creamos un apoyo a partir del movimiento terminado
                            apoyos.llegadaApoyo(movimiento.id_Movimiento);
                        }
                        else
                        {
                            // El movimento es un ataque, llamamos al metodo que controla todo lo relevante a la batalla
                            calculoBatalla(movimiento.id_Movimiento);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        // Metodo que realiza todo lo necesario para el procesamiento de una batalla
        public void calculoBatalla(int id_Movimiento)
        {
            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    var infoAtaque = (from register in context.Movimientos
                                     where id_Movimiento == register.id_Movimiento && 
                                     register.vencedor == -1
                                     select register).FirstOrDefault();

                    if (infoAtaque != null)
                    {
                        var infoTropa = from register in context.Tropas
                                        orderby register.id_Tropas
                                        select new
                                        {
                                            register.poblacion,
                                            register.potencia
                                        };

                        var tropas = infoTropa.ToList();

                        var infoPuebloDefensor = (from register in context.Pueblo
                                             where register.id_Pueblo == infoAtaque.puebloDestino
                                             select register).FirstOrDefault();

                        var infoPuebloAtacante = (from register in context.Pueblo
                                              where register.id_Pueblo == infoAtaque.puebloOrigen
                                              select register).FirstOrDefault();

                        var infoApoyos = from register in context.Apoyos
                                         where register.puebloDestino == infoAtaque.puebloDestino &&
                                         register.horaFin > DateTime.Now
                                         select register;

                        servicioManejoPueblos servicioPueblo  = new servicioManejoPueblos();
                        servicioManejoApoyos servicioApoyo = new servicioManejoApoyos();

                        int potenciaDefJugador = (int)infoPuebloDefensor.arqueros * tropas[0].potencia + (int)infoPuebloDefensor.ballesteros * tropas[1].potencia;

                        int potenciaAtqJugador = (int)infoAtaque.piqueros * tropas[2].potencia + (int)infoAtaque.caballeros * tropas[3].potencia + (int)infoAtaque.paladines * tropas[4].potencia;

                        int resultadoBatalla = potenciaAtqJugador - potenciaDefJugador;

                        if (resultadoBatalla <= 0)
                        {
                            resultadoBatalla = resultadoBatalla * -1;
                            double ratioPerdidas = Math.Round((double)potenciaDefJugador / resultadoBatalla, 2);

                            if (resultadoBatalla == 0)
                            {
                                infoAtaque.vencedor = 0;
                            }
                            else
                            {
                                infoAtaque.vencedor = 1;

                            }

                            infoPuebloAtacante.poblacion += infoAtaque.piqueros * tropas[2].poblacion + infoAtaque.caballeros * tropas[3].poblacion + infoAtaque.paladines * tropas[4].poblacion;

                            tropasDefensivasEntity tropasDefensivas = servicioPueblo.obtenerDefRealPueblo(infoPuebloDefensor.id_Pueblo);
                            infoPuebloDefensor.arqueros -= (int)Math.Round(tropasDefensivas.arqueros / ratioPerdidas);
                            infoPuebloDefensor.ballesteros -= (int)Math.Round(tropasDefensivas.ballesteros / ratioPerdidas);
                            infoPuebloDefensor.poblacion += (int)Math.Round(tropasDefensivas.arqueros / ratioPerdidas) * tropas[0].poblacion
                                + (int)Math.Round(tropasDefensivas.ballesteros / ratioPerdidas) * tropas[1].poblacion;

                            foreach ( var apoyo in infoApoyos)
                            {
                                int arquerosPerdidos = (int)Math.Round((int)apoyo.arqueros / ratioPerdidas);
                                int ballesterosPerdidos = (int)Math.Round((int)apoyo.ballesteros / ratioPerdidas);

                                servicioApoyo.actualizarApoyo(apoyo.id_Apoyo, arquerosPerdidos, ballesterosPerdidos);
                            }


                        }
                        else
                        {
                            double ratioPerdidas = Math.Round((double)potenciaDefJugador / resultadoBatalla, 2);

                            infoAtaque.vencedor = 2;

                            int piqueros = (int)Math.Round((int)infoAtaque.piqueros / ratioPerdidas);
                            int caballeros = (int)Math.Round((int)infoAtaque.piqueros / ratioPerdidas);
                            int paladines = (int)Math.Round((int)infoAtaque.piqueros / ratioPerdidas);

                            infoPuebloAtacante.piqueros += piqueros;
                            infoPuebloAtacante.caballeros += caballeros;
                            infoPuebloAtacante.paladines += paladines;
                            infoPuebloAtacante.poblacion += piqueros * tropas[2].poblacion + caballeros * tropas[3].poblacion + paladines * tropas[4].poblacion;

                            tropasDefensivasEntity tropasDefensivas = servicioPueblo.obtenerDefRealPueblo(infoPuebloDefensor.id_Pueblo);
                            infoPuebloDefensor.arqueros -= tropasDefensivas.arqueros;
                            infoPuebloDefensor.ballesteros -= tropasDefensivas.ballesteros;
                            infoPuebloDefensor.poblacion += tropasDefensivas.arqueros * tropas[0].poblacion + tropasDefensivas.ballesteros * tropas[1].poblacion;

                            foreach (var apoyo in infoApoyos)
                            {
                                int arquerosPerdidos = (int)apoyo.arqueros;
                                int ballesterosPerdidos = (int)apoyo.ballesteros;

                                servicioApoyo.actualizarApoyo(apoyo.id_Apoyo, arquerosPerdidos, ballesterosPerdidos);
                            }

                            if (paladines > 0)
                            {
                                servicioPueblo.cambiarPropietarioPueblo(infoPuebloDefensor.id_Pueblo, infoPuebloAtacante.propietario);
                            }
                        }

                    }
                }
            }
            catch
            {
                
            }
        }
    }
}

// Metodos: RealizarApoyo; RealizarAtaque; ObtenerMovimientos; LlegadaMovimiento; Batalla; ObtenerVencedor;

    // Mira con la clase timer deberias poder hacer guay lo de los metodos de llegada de movimientos, pero ojo por que habría que implementar el async ese
    // Si no podemos hacer eso hay que tener cuidado de que no llamen 2 clientes al mismo servicio por la misma batalla