﻿using System;
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
                using (var context = new ProyectoDAMEntitis())
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
                using (var context = new ProyectoDAMEntitis())
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
    }
}