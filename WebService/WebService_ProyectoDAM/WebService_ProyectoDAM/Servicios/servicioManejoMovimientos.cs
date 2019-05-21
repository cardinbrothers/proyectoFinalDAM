using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

                    // Creamos un hilo aparte que trata la finalizacion del movimiento
                    Task.Run(() => llegadaMovimiento(insertMovimiento.duracion, insertMovimiento.id_Movimiento));

                }
            }
            catch
            {

            }
        }

        // Metodo en segundo plano que gestiona la llegada del movimiento
        public async void llegadaMovimiento(TimeSpan tiempo, int id_movimiento)
        {
            await Task.Run(() =>
            {
                try
                {
                    // Mandamos al hilo esperar por que pase el tiempo indicado
                    Thread.Sleep(tiempo);

                    // Llamamos al metodo que comprueba que haya acabado el movimiento y realiza lo correspondiente
                    comprobarMovimientoAcabado(id_movimiento);                  
                }
                catch
                {

                }

            });
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
                        movimientoAux.arqueros = movimiento.arqueros;
                        movimientoAux.ballesteros = movimiento.ballesteros;
                        movimientoAux.piqueros = movimiento.piqueros;
                        movimientoAux.caballeros = movimiento.caballeros;
                        movimientoAux.paladines = movimiento.paladines;
                        movimientoAux.vencedor = movimiento.vencedor;

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

        // Metodo que comprueba si un movimentos han ocurrido ya y llama a los metodos que realicen las acciones correspondientes
        public void comprobarMovimientoAcabado(int id_movimiento)
        {
            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos la lista de movimientos terminados
                    var movimentosAcabados = from register in context.Movimientos
                                             where register.id_Movimiento == id_movimiento &&
                                             register.horaLlegada < DateTime.Now &&
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
                    // Obtenemos la informacion del movimiento de la base de datos
                    var infoAtaque = (from register in context.Movimientos
                                     where id_Movimiento == register.id_Movimiento && 
                                     register.vencedor == -1
                                     select register).FirstOrDefault();

                    // Comprobamos si el movimiento existe
                    if (infoAtaque != null)
                    {
                        // Obtenemos la informacion de las tropas de la base de datos
                        var infoTropa = from register in context.Tropas
                                        orderby register.id_Tropas
                                        select new
                                        {
                                            register.poblacion,
                                            register.potencia
                                        };

                        // Pasamos los registros de tropas de la base de datos a una lista
                        var tropas = infoTropa.ToList();

                        // Obtenemos la informacion del pueblo defensor de la batalla
                        var infoPuebloDefensor = (from register in context.Pueblo
                                             where register.id_Pueblo == infoAtaque.puebloDestino
                                             select register).FirstOrDefault();

                        // Obtenemos la informacion del pueblo atacante de la batalla 
                        var infoPuebloAtacante = (from register in context.Pueblo
                                              where register.id_Pueblo == infoAtaque.puebloOrigen
                                              select register).FirstOrDefault();

                        // Obtenemos los apoyos destinados en el pueblo defensor de la batalla
                        var infoApoyos = from register in context.Apoyos
                                         where register.puebloDestino == infoAtaque.puebloDestino &&
                                         register.horaFin > DateTime.Now
                                         select register;

                        // Creamos los objetos de los servicios de manejo de pueblos y apoyos para llamar a sus metodos
                        servicioManejoPueblos servicioPueblo  = new servicioManejoPueblos();
                        servicioManejoApoyos servicioApoyo = new servicioManejoApoyos();

                        // Calculamos la potencia total del jugador defensor multiplicando las tropas en su pueblo por su potencia
                        int potenciaDefJugador = infoPuebloDefensor.arqueros * tropas[0].potencia + infoPuebloDefensor.ballesteros * tropas[1].potencia;

                        // Calculamos la potencia total del jugador atacante multiplicando las tropas en el movimiento por su potencia
                        int potenciaAtqJugador = infoAtaque.piqueros * tropas[2].potencia + infoAtaque.caballeros * tropas[3].potencia + infoAtaque.paladines * tropas[4].potencia;

                        // Calculamos el resultado de la batalla restando la potencia del atacante por la potencia del defensor
                        int resultadoBatalla = potenciaAtqJugador - potenciaDefJugador;

                        // Comprobamos si el resultado es menor que 0 (el defensor es quien gana) (si es igual a 0 es un empate)
                        if (resultadoBatalla <= 0)
                        {
                            // Paramos el resultado de batalla a positivo para realizar los calculos necesarios
                            resultadoBatalla = resultadoBatalla * -1;

                            // Calculamos el ratio de perdidas dividiendo la potencia original del defensor por el resultado de batalla y lo redondeamos a 2 decimales
                            double ratioPerdidas = Math.Round((double)potenciaDefJugador / resultadoBatalla, 2);

                            // Comprobamos si el resultado fue un empate o victoria del defensor
                            if (resultadoBatalla == 0)
                            {
                                // En caso de empate cambiamos el campo de vencedor de ataque a 0
                                infoAtaque.vencedor = 0;
                            }
                            else
                            {
                                // En caso de victoria del defensor cambiamos el campo de vencedor de ataque a 0
                                infoAtaque.vencedor = 1;

                            }

                            // Cambiamos la poblacion del pueblo atacante para sumarle la poblacion que ocupaban las tropas perdidas en el ataque
                            infoPuebloAtacante.poblacion += infoAtaque.piqueros * tropas[2].poblacion + infoAtaque.caballeros * tropas[3].poblacion + infoAtaque.paladines * tropas[4].poblacion;

                            // Obtenemos las tropas propias del pueblo, (sin contar apoyos) y les restamos las tropas perdidas 
                            //(calculadas restando las propias a la division del ratio de perdidas al numero de tropas)
                            // del total, además de sumar la poblacion de las tropas perdidas
                            tropasDefensivasEntity tropasDefensivas = servicioPueblo.obtenerDefRealPueblo(infoPuebloDefensor.id_Pueblo);
                            int arquerosPerdidos = tropasDefensivas.arqueros - (int)Math.Round(tropasDefensivas.arqueros / ratioPerdidas);
                            int ballesterosPerdidos = tropasDefensivas.ballesteros - (int)Math.Round(tropasDefensivas.ballesteros / ratioPerdidas);

                            infoPuebloDefensor.arqueros -= arquerosPerdidos;
                            infoPuebloDefensor.ballesteros -= ballesterosPerdidos;
                            infoPuebloDefensor.poblacion += arquerosPerdidos * tropas[0].poblacion + ballesterosPerdidos * tropas[1].poblacion;

                            // Por cada apoyo en los apoyos destinados en el pueblo defensor ==>
                            foreach ( var apoyo in infoApoyos)
                            {
                                // Calculamos las tropas perdidas del apoyo en concreto
                                arquerosPerdidos = apoyo.arqueros - (int)Math.Round(apoyo.arqueros / ratioPerdidas);
                                ballesterosPerdidos = apoyo.ballesteros - (int)Math.Round(apoyo.ballesteros / ratioPerdidas);

                                // Llamamos al metodo encargado de realizar las acciones correspondientes a la perdida de tropas del apoyo
                                servicioApoyo.actualizarApoyo(apoyo.id_Apoyo, arquerosPerdidos, ballesterosPerdidos);
                            }


                        }
                        else
                        {
                            // Calculamos el ratio de perdidas
                            double ratioPerdidas = Math.Round((double)potenciaDefJugador / resultadoBatalla, 2);

                            // Cambiamos el campo de vencedor del movimiento a 2 porque el vencedor es el atacante
                            infoAtaque.vencedor = 2;

                            // Calculamos las tropas supervivientes de cada tipo
                            int piquerosSupervivientes = (int)Math.Round(infoAtaque.piqueros / ratioPerdidas);
                            int caballerosSupervivientes = (int)Math.Round(infoAtaque.piqueros / ratioPerdidas);
                            int paladinessupervivientes = (int)Math.Round(infoAtaque.piqueros / ratioPerdidas);

                            // Sumamos las tropas supervivientes al pueblo atacante y restamos la poblacion de aquellas que no sobrevivieron
                            infoPuebloAtacante.piqueros += piquerosSupervivientes;
                            infoPuebloAtacante.caballeros += caballerosSupervivientes;
                            infoPuebloAtacante.paladines += paladinessupervivientes;
                            infoPuebloAtacante.poblacion += (infoAtaque.piqueros - piquerosSupervivientes) * tropas[2].poblacion 
                                + (infoAtaque.caballeros - caballerosSupervivientes) * tropas[3].poblacion 
                                + (infoAtaque.paladines - paladinessupervivientes) * tropas[4].poblacion;

                            // Obtenemos las tropas propias del pueblo defensor y le restamos al total estas puesto que pierden tpdas y sumamos la poblacion que se libera
                            tropasDefensivasEntity tropasDefensivas = servicioPueblo.obtenerDefRealPueblo(infoPuebloDefensor.id_Pueblo);
                            infoPuebloDefensor.arqueros -= tropasDefensivas.arqueros;
                            infoPuebloDefensor.ballesteros -= tropasDefensivas.ballesteros;
                            infoPuebloDefensor.poblacion += tropasDefensivas.arqueros * tropas[0].poblacion + tropasDefensivas.ballesteros * tropas[1].poblacion;

                            // Por cada apoyo hacemos lo propio
                            foreach (var apoyo in infoApoyos)
                            {
                                // Obtenemos las tropas perdidas
                                int arquerosPerdidos = apoyo.arqueros;
                                int ballesterosPerdidos = apoyo.ballesteros;

                                // Llamamos al metodo que actualiza las tropas del apoyo
                                servicioApoyo.actualizarApoyo(apoyo.id_Apoyo, arquerosPerdidos, ballesterosPerdidos);
                            }

                            // Comprobamos si sobrevivió al menos un paladin
                            if (paladinessupervivientes > 0)
                            {
                                // En caso de que sobreviviera un paladin llamamos al metod que realiza el cambio de poseion del pueblo
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

        // Metodo que devuelve un codigo con el vencedor de la batalla
        public int obtenedorVencedorBatalla(int id_movimiento)
        {
            // Codigo que devolveremos con el vencedor de la batalla:
            // -2 --> Batalla inexistente
            // -1 --> No ha ocurrido aun la batalla
            // 0 --> Empate
            // 1 --> El vencedor es el defensor
            // 2 --> El vencedor es el atacante
            int resultadoMovimiento = -2;

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos la informacion del movimiento de la base de datos
                    var resultadoVencedor = (from register in context.Movimientos
                                            where register.id_Movimiento == id_movimiento && 
                                            register.tipoMovimiento == "ataque"
                                            select register).FirstOrDefault();

                    // Comprobamos que exista el movimiento
                    if (resultadoVencedor != null)
                    {
                        // Almacenamos el codigo del vencedor en la variable que devolvemos
                        resultadoMovimiento = resultadoVencedor.vencedor;
                    }
                }
            }
            catch
            {

            }

            // Devolvemos la variable con el codigo del vencedor
            return resultadoMovimiento;
        }

    }
}