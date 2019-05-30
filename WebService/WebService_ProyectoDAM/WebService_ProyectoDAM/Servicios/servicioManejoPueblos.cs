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
            ejex = rnd.Next(11);
            ejey = rnd.Next(11);

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
                        puebloObtenido.poblacionRestante = infoPueblo.poblacion;
                        puebloObtenido.coordenadas = infoPueblo.coordenadas;
                        puebloObtenido.arqueros = infoPueblo.arqueros;
                        puebloObtenido.ballesteros = infoPueblo.ballesteros;
                        puebloObtenido.piqueros = infoPueblo.piqueros;
                        puebloObtenido.caballeros = infoPueblo.caballeros;
                        puebloObtenido.paladines = infoPueblo.paladines;

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
                        puebloAux.poblacionRestante = pueblo.poblacion;
                        puebloAux.coordenadas = pueblo.coordenadas;
                        puebloAux.arqueros = pueblo.arqueros;
                        puebloAux.ballesteros = pueblo.ballesteros;
                        puebloAux.piqueros = pueblo.piqueros;
                        puebloAux.caballeros = pueblo.caballeros;
                        puebloAux.paladines = pueblo.paladines;

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

                    DateTime tiempoActual = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Europe Standard Time");
                    // Obtenemos los apoyos originados del pueblo
                    var apoyos = from register in context.Apoyos
                                 where register.puebloOrigen == id_pueblo &&
                                 register.horaFin > tiempoActual
                                 select register;

                    // Obtenemos los movimientos originados del pueblo
                    var movimientos = from register in context.Movimientos
                                      where register.puebloOrigen == id_pueblo &&
                                      register.horaLlegada > tiempoActual &&
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
                        potenciaPueblo += pueblo.arqueros * listaPotencia[0];
                        potenciaPueblo += pueblo.ballesteros * listaPotencia[1];
                        potenciaPueblo += pueblo.piqueros * listaPotencia[2];
                        potenciaPueblo += pueblo.caballeros * listaPotencia[3];
                        potenciaPueblo += pueblo.paladines * listaPotencia[4];

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
                    tropasDefensivas.arqueros = pueblo.arqueros;
                    tropasDefensivas.ballesteros = pueblo.ballesteros;

                    // Tratamos cada apoyo con un foreach
                    foreach (var apoyo in apoyos)
                    {
                        // Restamos las tropas del apoyo al total del pueblo 
                        tropasDefensivas.arqueros -= apoyo.arqueros;
                        tropasDefensivas.ballesteros -= apoyo.ballesteros;
                    }

                }
            }
            catch
            {

            }

            // Devolvemos el objeto con las tropas propias del pueblo
            return tropasDefensivas;
        }

        // Metodo que obtiene el tiempo que tardan las tropas en ir de una coordinada a otra
        public TimeSpan obtenerDistancia(string coordenada1, string coordenada2, int id_Partida)
        {
            // Creamos un objeto TimeSpan para devolver con el tiempo que se tarda en realizar el movimietno
            TimeSpan tiempoDistancia = new TimeSpan();

            try
            {
                // Creamos un array de caracteres para separar una cadena por "," "(" y ")" puesto que las coordenadas estan en formato (x,y)
                char[] separador = { ',', '(', ')' };

                // Obenemos los valores de ambos ejes de las dos coordenadas
                string[] ejes1 = coordenada1.Split(separador, StringSplitOptions.RemoveEmptyEntries);
                string[] ejes2 = coordenada2.Split(separador, StringSplitOptions.RemoveEmptyEntries);

                // Pasamos a enteros los valores de los ejes
                int x1 = Convert.ToInt32(ejes1[0]);
                int y1 = Convert.ToInt32(ejes1[1]);
                int x2 = Convert.ToInt32(ejes2[0]);
                int y2 = Convert.ToInt32(ejes2[1]);

                // Calculamos la distancia gracias al teorema de pitagoras
                double distancia = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
                distancia = Math.Sqrt(distancia);

                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos la variable de velocidad de la base de datos
                    var variableVelocidad = (from register in context.Partida
                                             where register.id_Partida == id_Partida &&
                                             register.activo == true
                                             select register.Velocidad).FirstOrDefault();

                    // Comprobamos si existe la partida
                    if (variableVelocidad != 0)
                    {
                        // Calculamos los minutos multiplicando la distancia por 0.5 y por la velocidad de la partida
                        int minutos = (int)Math.Round(distancia * 0.5 * variableVelocidad);

                        // Iniciamos el objeto TimeSpan con los minutos calculados
                        tiempoDistancia = new TimeSpan(0, minutos, 0);
                    }
                    
                }

               
            }
            catch
            {

            }

            // Devolvemos el objeto con el tiempo que se tarda
            return tiempoDistancia;
        }

        // Metodo para obtener el id de un pueblo a partir de sus coordenadas
        public int obtenerId(string coords, int id_Partida)
        {
            // Creamos la variable entera donde almacenaremos el id del pueblo, -1 si no existe
            int id_pueblo = -1;

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos el id de la base de datos
                    id_pueblo = (from register in context.Pueblo
                                 from register2 in context.Jugador
                                 where register.propietario == register2.nombreUsuario &&
                                 register.coordenadas == coords &&
                                 register2.id_Partida == id_Partida
                                 select register.id_Pueblo).FirstOrDefault();
                }
            }
            catch
            {

            }

            // Devolvemos el id
            return id_pueblo;
        }
    }
}