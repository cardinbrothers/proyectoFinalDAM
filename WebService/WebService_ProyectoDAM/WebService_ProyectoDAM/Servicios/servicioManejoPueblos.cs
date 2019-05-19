using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_ProyectoDAM.Models;
using WebService_ProyectoDAM.ApiEntities;

namespace WebService_ProyectoDAM.Servicios
{
    public class servicioManejoPueblos
    {
        public int crearPueblo(string nombreJugdor)
        {
            
            int error = 0; // Variable error que devolveremos con valor 0 si sale todo correcto y valor 1 si falla algo
            string coordenadas; // Variable coordenadas que crearemos aleatoriamente el formato es (x,y)

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos el limte de poblacion de la partida donde se crea el pueblo
                    var limitePoblacion = (from register in context.Partida
                                           from register2 in context.Jugador
                                           where register.id_Partida == register2.id_Partida &&
                                           register.activo == true &&
                                           register2.nombreUsuario == nombreJugdor
                                           select new
                                           {
                                               register.limitePoblacion,
                                               register.id_Partida
                                           }).FirstOrDefault();

                    // Pasamos las coordenadas obteneidas a un objeto de tipo lista de cadenas
                    List<string> coordenadasPartida = obtenerCoordsPartida(limitePoblacion.id_Partida);

                    // Generar coordenadas aleatoriaas (metodo)
                    coordenadas = generarCoordsAleatorias();

                    // Comprobamos que las coordenas generadas no existan ya ne la base de datos
                    while (coordenadasPartida.Contains(coordenadas))
                    {
                        coordenadas = generarCoordsAleatorias();
                    }

                    // Creamos el objeto para realizar el insert en la base de datos
                    Pueblo puebloCreado = new Pueblo();

                    // Introducimos los parametros del pueblo
                    puebloCreado.propietario = nombreJugdor;
                    puebloCreado.poblacion = limitePoblacion.limitePoblacion;
                    puebloCreado.arqueros = 0;
                    puebloCreado.ballesteros = 0;
                    puebloCreado.piqueros = 0;
                    puebloCreado.caballeros = 0;
                    puebloCreado.paladines = 0;
                    puebloCreado.coordenadas = coordenadas;

                    // Añadimos el pueblo nuevo a la base de datos
                    context.Pueblo.Add(puebloCreado);
                    context.SaveChanges();

                }
            }
            catch
            {
                // Error desconocido
                error = 1;
            }

            // Devolvemos el codigo  del error
            return error;
        }

        // Metodo que genera coordenadas aleatorias
        private string generarCoordsAleatorias()
        {
            // Creamos las variables necesarias
            int ejex, ejey;
            Random rnd = new Random();
            string coordenadas;

            // Obtenemos un numeo aleatorio del 0 al 20 para los ejes
            ejex = rnd.Next(21);
            ejey = rnd.Next(21);

            // Creamos la coordenada con los valores de ambos ejes
            coordenadas = String.Format("({0},{1})", ejex, ejey);

            // Devolvemos las coordenadas
            return coordenadas;

        }

        // Metodo que devuelve los parametros de un pueblo
        public puebloEntity obtenerPueblo (int id_Pueblo)
        {
            // Creamos el objeto de pueblo
            puebloEntity puebloObtenido = null;

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos el pueblo de la base de datos
                    var infoPueblo = (from register in context.Pueblo
                                      where register.id_Pueblo == id_Pueblo
                                      select register).FirstOrDefault();

                    // Coomprobamos si existe
                    if (infoPueblo != null)
                    {
                        puebloObtenido = new puebloEntity();

                        // Introducimos los datos del pueblo en el objeto a devolver
                        puebloObtenido.propietario = infoPueblo.propietario;
                        puebloObtenido.poblacionRestante = (int)infoPueblo.poblacion;
                        puebloObtenido.coordenadas = infoPueblo.coordenadas;
                        puebloObtenido.arqueros = (int)infoPueblo.arqueros;
                        puebloObtenido.ballesteros = (int)infoPueblo.ballesteros;
                        puebloObtenido.piqueros = (int)infoPueblo.piqueros;
                        puebloObtenido.caballeros = (int)infoPueblo.caballeros;
                        puebloObtenido.paladines = (int)infoPueblo.paladines;

                    }


                }
            }
            catch
            {

            }

            // Devolvemos los datos del pueblo
            return puebloObtenido;
        }

        // Metodo que devuelve los parametros de todos los pueblos de un jugador
        public List<puebloEntity> obtenerListaPueblos(string jugadorPropietario)
        {
            // Creamos la lista de pueblos
            List<puebloEntity> listaPueblos = new List<puebloEntity>();

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos los pueblo de la base de datos
                    var infoPueblos = from register in context.Pueblo
                                      where register.propietario == jugadorPropietario
                                      select register;

                    // Para cada pueblo obtenido añadimos sus datos a la lista
                    foreach (var pueblo in infoPueblos)
                    {
                        // Creamos un objeto auxiliar
                        puebloEntity puebloAux = new puebloEntity();

                        // Introducimos los datos del pueblo en el objeto auxiliar
                        puebloAux.id_Pueblo = pueblo.id_Pueblo;
                        puebloAux.propietario = pueblo.propietario;
                        puebloAux.poblacionRestante = (int)pueblo.poblacion;
                        puebloAux.coordenadas = pueblo.coordenadas;
                        puebloAux.arqueros = (int)pueblo.arqueros;
                        puebloAux.ballesteros = (int)pueblo.ballesteros;
                        puebloAux.piqueros = (int)pueblo.piqueros;
                        puebloAux.caballeros = (int)pueblo.caballeros;
                        puebloAux.paladines = (int)pueblo.paladines;

                        // Añadimos el objeto auxiliar a la lista
                        listaPueblos.Add(puebloAux);

                    }


                }
            }
            catch
            {

            }

            // Devolvemos la lista de pueblos
            return listaPueblos;
        }

        // Metodo para cambiar el propietario del pueblo
        public void cambiarPropietarioPueblo(int id_pueblo, string nuevoPropietario)
        {
            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos el registro del pueblo que hay que cambiar
                    var puebloCambiar = (from register in context.Pueblo
                                         where register.id_Pueblo == id_pueblo
                                         select register).FirstOrDefault();

                    // Obtenemos el limite de poblacion de la partida
                    var limitePoblacion = (from register in context.Partida
                                           from register2 in context.Jugador
                                           where register.id_Partida == register2.id_Partida &&
                                           register.activo == true &&
                                           register2.nombreUsuario == nuevoPropietario
                                           select register.limitePoblacion).FirstOrDefault();

                    // Obtenemos los apoyos originados del pueblo
                    var apoyos = from register in context.Apoyos
                                 where register.puebloOrigen == id_pueblo &&
                                 register.horaFin > DateTime.Now
                                 select register;

                    // Obtenemos los movimientos originados del pueblo
                    var movimientos = from register in context.Movimientos
                                      where register.puebloOrigen == id_pueblo &&
                                      register.horaLlegada > DateTime.Now &&
                                      register.vencedor == -1
                                      select register;

                    // Creamos un objeto del servicio de apoyos y finalizamos los apoyos del puebllo
                    servicioManejoApoyos manejoApoyos = new servicioManejoApoyos();
                    foreach (var apoyo in apoyos)
                    {
                        manejoApoyos.apoyoFinalizado(apoyo.id_Apoyo);
                    }

                    // Cancelamos los movimientos del pueblo estableciendo el campo de vencedor a 3
                    foreach (var movimiento in movimientos)
                    {
                        movimiento.vencedor = 3;
                    }

                    // Cambiamos los valores del pueblo
                    puebloCambiar.propietario = nuevoPropietario;
                    puebloCambiar.poblacion = limitePoblacion;
                    puebloCambiar.arqueros = 0;
                    puebloCambiar.ballesteros = 0;
                    puebloCambiar.piqueros = 0;
                    puebloCambiar.caballeros = 0;
                    puebloCambiar.paladines = 0;

                    // Guardamos los cambios
                    context.SaveChanges();

                }
            }
            catch
            {

            }
        }

        // Metodo para obtener todas las coordenadas de los pueblos de una partida
        public List<string> obtenerCoordsPartida(int id_partida)
        {
            // Creamos una lista donde alamacenar las corrdenadas
            List<string> coordenadasPueblos = null;

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // obtenemos una lista de las coordenadas que existen en la partida actualmente
                    var listaCoordenadas = from register in context.Jugador
                                           from register2 in context.Pueblo
                                           where register.nombreUsuario == register2.propietario &&
                                           register.id_Partida == id_partida
                                           select register2.coordenadas;

                    // Convertimos el resultado de la sentencia a tipo lista
                    coordenadasPueblos = listaCoordenadas.ToList();
                }
            }
            catch
            {

            }

            // Devolvemos la lista de coordenadas de pueblos
            return coordenadasPueblos;
        }

        // Metodo que devuelve la potencia total de cada jugador
        public int obtenerPotenciaJugador(string jugadorPropietario)
        {
            // Creamos la variable para almacenar la potenciaTotal
            int potenciaTotal = 0;

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos los pueblos del jugador de la base de datos
                    var infoPueblos = from register in context.Pueblo
                                      where register.propietario == jugadorPropietario
                                      select register;

                    // Obtenemos los valores de potencia de cada tropa
                    var potenciaTropas = from register in context.Tropas
                                         orderby register.id_Tropas
                                         select register.potencia;

                    // convertimos a lista los valores de potencia de cada tropa
                    var listaPotencia = potenciaTropas.ToList();

                    // Para cada pueblo obtenido añadimos su potencia a la potencia total
                    foreach (var pueblo in infoPueblos)
                    {
                        // Un pueblo de base aporta 10 de potencia
                        int potenciaPueblo = 10;

                        // Añadimos la potencia de las tropas del pueblo a la suma 
                        potenciaPueblo += (int)pueblo.arqueros * listaPotencia[0];
                        potenciaPueblo += (int)pueblo.ballesteros * listaPotencia[1];
                        potenciaPueblo += (int)pueblo.piqueros * listaPotencia[2];
                        potenciaPueblo += (int)pueblo.caballeros * listaPotencia[3];
                        potenciaPueblo += (int)pueblo.paladines * listaPotencia[4];

                        // Añadimos la potencia del pueblo a la total
                        potenciaTotal += potenciaPueblo;
                    }
                }
            }
            catch
            {

            }

            // Devolvemos la lista de pueblos
            return potenciaTotal;
        }

        // Metodo que obtiene las tropas defensivas propias de un pueblo
        public tropasDefensivasEntity obtenerDefRealPueblo(int id_Pueblo)
        {
            // Creamos el objeto que devolveremos con las tropas defensivas del pueblo
            tropasDefensivasEntity tropasDefensivas = new tropasDefensivasEntity();

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos las tropas defensivas totales del pueblo
                    var pueblo = (from register in context.Pueblo
                                  where register.id_Pueblo == id_Pueblo
                                  select new
                                  {
                                      register.arqueros,
                                      register.ballesteros
                                  }).FirstOrDefault();

                    // Obtenemos todas las tropas apoyadas en ese pueblo
                    var apoyos = from register in context.Apoyos
                                 where register.puebloDestino == id_Pueblo
                                 select new
                                 {
                                     register.arqueros,
                                     register.ballesteros
                                 };

                    // Introducimos las tropas defensivas totales del pueblo en el objeto que devolveremos
                    tropasDefensivas.arqueros = (int)pueblo.arqueros;
                    tropasDefensivas.ballesteros = (int)pueblo.ballesteros;

                    // Tratamos cada apoyo con un foreach
                    foreach (var apoyo in apoyos)
                    {
                        // Restamos las tropas del apoyo al total del pueblo 
                        tropasDefensivas.arqueros -= (int)apoyo.arqueros;
                        tropasDefensivas.ballesteros -= (int)apoyo.ballesteros;
                    }

                }
            }
            catch
            {

            }

            // Devolvemos el objeto con las tropas propias del pueblo
            return tropasDefensivas;
        }
    }
}