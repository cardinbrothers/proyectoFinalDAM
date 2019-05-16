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

        //-------------------------------------------
        //
        // Hay que añadir variable id_partida a los pueblos? 
        //
        //-------------------------------------------


        public int crearPueblo(string nombreJugdor)
        {
            
            int error = 0; // Variable error que devolveremos con valor 0 si sale todo correcto y valor 1 si falla algo
            string coordenadas; // Variable coordenadas que crearemos aleatoriamente el formato es (x,y)

            try
            {
                using (var context = new ProyectoDAMEntitis())
                {
                    // Obtenemos el limte de poblacion de la partida donde se crea el pueblo
                    var limitePoblacion = (from register in context.Partida
                                           from register2 in context.Jugador
                                           where register.id_Partida == register2.id_Partida &&
                                           register.activo == true &&
                                           register2.nombreUsuario == nombreJugdor
                                           select register.limitePoblacion).FirstOrDefault();

                    // obtenemos una lista de las coordenadas que existen en la partida actualmente
                    var listaCoordenadas = from register in context.Partida
                                           from register2 in context.Jugador
                                           from register3 in context.Pueblo
                                           where register.id_Partida == register2.id_Partida &&
                                           register.activo == true &&
                                           register2.nombreUsuario == register3.propietario
                                           select register3.coordenadas;

                    // Pasamos las coordenadas obteneidas a un objeto de tipo lista de cadenas
                    List<string> coordenadasPartida = listaCoordenadas.ToList();

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
                    puebloCreado.poblacion = limitePoblacion;
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

        //_ Metodo que genera coordenadas aleatorias
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
    }
}