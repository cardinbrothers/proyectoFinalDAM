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

    // OYE!!! QUITAR PARAMETROS PARTIDA lo del tipo y añaidr ganador a la tabla de la base de datos          

    public class servicioMenuPrincipal
    {
        // Metodo que crea una partida a partir de los parametros recibidos
        public int crearPartida(infoPartidaEntity record)
        {
            // Variable para devolver un codigo de error 0--> Todo correcto; 
            //1 --> Error desconocido
            int error = 0;
            int id_PartidaNueva;
            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos el id mayor hasta la fecha  
                    // Esto que oH?? ??  Cambialo cuando crees la base de datos de nuevo
                    var idPartidas = (from register in context.Partida
                                      orderby register.id_Partida descending
                                      select new
                                      {
                                          register.id_Partida
                                      }).FirstOrDefault();

                    // Comprobamos si ya hay un id de partida en la base de datos
                    if (idPartidas != null)
                    {
                        // Introduciremos el id sumando uno más al último
                        id_PartidaNueva = idPartidas.id_Partida + 1;
                    }
                    else
                    {
                        // Introducimos el primer id empezando por 0
                        id_PartidaNueva = 1;
                    }

                    // Creamos el objeto para insertar la nueva partida
                    Partida partidaCreada = new Partida();

                    // Introducimos los parametros de la partida
                    partidaCreada.id_Partida = id_PartidaNueva;
                    partidaCreada.Modalidad = record.modalidad;
                    partidaCreada.Velocidad = record.velocidad;
                    partidaCreada.Duracion = record.duracion;
                    partidaCreada.limiteJugadores = record.limiteJugadores;
                    partidaCreada.limitePoblacion = record.limitePoblacion;
                    partidaCreada.fechaInicio = DateTime.Now;
                    partidaCreada.activo = true;

                    // Insertamos la partida
                    context.Partida.Add(partidaCreada);
                    context.SaveChanges();

                    // Creamos un hilo aparte que desactivará la partida cuando esta acabe
                    Task.Run(() => desactivarPartida(partidaCreada.Duracion, partidaCreada.id_Partida));

                }
            }
            catch
            {
                error = 1;
            }

            // Devolvemos el codigo de error
            return error;
        }
        
        // Metodo que desactiva una partida despues de esperar el tiempo indicado
        public async void desactivarPartida(TimeSpan tiempo, int id_Partida)
        {
            await Task.Run(() =>
            {
                try
                {

                    // Mandamos al hilo esperar por que pase el tiempo indicado
                    Thread.Sleep(tiempo);

                    using (var context = new ProyectoDAMEntities())
                    {
                        // Obtenemos el registro de la partida
                        var partida = (from register in context.Partida
                                       where register.id_Partida == id_Partida
                                       select register).FirstOrDefault();

                        // Comprobamos que el registro no sea nulo
                        if (partida != null)
                        {
                            // Establecemos el campo activo a falso
                            partida.activo = false;

                            // Confirmamos los cambios a la base de datos
                            context.SaveChanges();
                        }
                    }
                }
                catch
                {

                }

            });
        }

        // Metodo que obtiene todas las partidas activas atualmente
        public List<infoPartidaEntity> partidasActivas()
        {
            // Creamos la lista para almacenar las partidas activas
            List<infoPartidaEntity> listaPartidasActivas = new List<infoPartidaEntity>();

            try
            {

                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos las partidas de la base de datos
                    var listaPartidas = from register in context.Partida
                                        where register.activo == true
                                        select new
                                        {
                                            register.id_Partida,
                                            register.Modalidad,
                                            register.Velocidad,
                                            register.Duracion,
                                            register.limiteJugadores,
                                            register.limitePoblacion,
                                            register.fechaInicio,
                                            jugadoresActivos = (from register2 in context.Jugador
                                                                where register2.id_Partida == register.id_Partida
                                                                select register2.nombreUsuario).Count()
                                        };

                    // Tratamos cada partida obtenida con un foreach
                    foreach (var partida in listaPartidas)
                    {
                        // Creamos un objeto de partida auxiliar para introducir los paramtros obtenidos de la BD
                        infoPartidaEntity partidaAux = new infoPartidaEntity();

                        // Introducimos los paramtros de una partida en el objeto auxiliar
                        partidaAux.id_Partida = partida.id_Partida;
                        partidaAux.modalidad = partida.Modalidad;
                        partidaAux.velocidad = partida.Velocidad;
                        partidaAux.duracion = partida.Duracion;
                        partidaAux.limiteJugadores = partida.limiteJugadores;
                        partidaAux.limitePoblacion = partida.limitePoblacion;
                        partidaAux.fechaInicio = partida.fechaInicio;
                        partidaAux.jugadoresActivos = partida.jugadoresActivos;

                        // Añadimos la partida a la lista
                        listaPartidasActivas.Add(partidaAux);
                    }
                }
            }
            catch
            {

            }

            // Devolvemos la lista de partidas activas
            return listaPartidasActivas;
        }

        // Metodo que crea un jugador segun la informacion recibida del usuario
        public int crearJugador(infoJugadorEntity jugador)
        {
            // Valor entero que almacena un digito segun que error ocurra siendo: 
            //      0 --> Todo correcto
            //      1 --> Nombre de usuario ya existente
            //      2 --> Limite de jugadores para la partida ya alcanzado
            //      3 --> Partida expirada
            //      4 --> Error desconocido
            int error = 0;

            try
            {

                using (var context = new ProyectoDAMEntities())
                {
                    // Combrobamos si existe ya un jugador con ese nombre de usuario
                    var jugadores = (from register in context.Jugador
                                     where register.nombreUsuario == jugador.nombreUsuario &&
                                     register.id_Partida == jugador.id_partida
                                     select new
                                     {
                                         register.nombreUsuario
                                     }).FirstOrDefault();

                    if (jugadores == null)
                    {
                        // Almacenamos el numero de jugadores actuales para la partida 
                        var numJugadores = (from register in context.Jugador
                                            where register.id_Partida == jugador.id_partida
                                            select register.nombreUsuario).Count();

                        // Almacenamos el limite de jugadores y los datos para calcular la hora de finalizacion de la partida
                        var partida = (from register in context.Partida
                                               where register.id_Partida == jugador.id_partida
                                               select new
                                               {
                                                   register.limiteJugadores,
                                                   register.fechaInicio,
                                                   register.Duracion
                                               }).FirstOrDefault();

                        // Comprobamos si se ha superado el limite de jugadores
                        if (numJugadores < partida.limiteJugadores)
                        {
                            // Comprobamos si la partida no se ha terminado aun
                            if (partida.fechaInicio.AddSeconds(Convert.ToInt32(partida.Duracion.TotalSeconds)) >= DateTime.Now)
                            {
                                // Introducimos la informacion del usuario en la base de datos
                                Jugador jugadorNuevo = new Jugador();
                                jugadorNuevo.nombreUsuario = jugador.nombreUsuario;
                                jugadorNuevo.contraseña = jugador.contraseña;
                                jugadorNuevo.id_Partida = jugador.id_partida;

                                context.Jugador.Add(jugadorNuevo);
                                context.SaveChanges();

                                // Creamos un pueblo para el jugador
                                servicioManejoPueblos objPueblos = new servicioManejoPueblos();
                                objPueblos.crearPueblo(jugadorNuevo.nombreUsuario);
                            }
                            else
                            {
                                // La partida ha acabado
                                error = 3;
                            }

                            
                        }
                        else
                        {
                            // Se ha superado el limite de jugadores de la partida
                            error = 2;
                        }


                    }
                    else
                    {
                        // Nombre usuario ya existente
                        error = 1;
                    }
                    
                }
            }
            catch
            {
                error = 4;
            }

            // Devolvemos la lista de partidas activas
            return error;
        }

        // Metodo para iniciar sesion con un Usuario y contraseña
        public int inciarSesion(infoJugadorEntity jugador)
        {
            // Valor entero que almacena un digito segun que error ocurra siendo: 
            //      0 --> Nombre usuario y contraseña correcta
            //      1 --> No existe el usuario para la partida indicada
            //      2 --> Contrasñe incorrecta
            //      3 --> Error desconocido

            int error;
            try
            {

                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos la contraseña de la base de datos
                    var contraseña = (from register in context.Jugador
                                     where register.nombreUsuario == jugador.nombreUsuario &&
                                     register.id_Partida == jugador.id_partida
                                     select register.contraseña).FirstOrDefault();

                    // Comprobamos si existe el usuario
                    if (contraseña != null)
                    {
                        // Creamos el objeto para comprobar las contraseñas
                        StringComparer comparador = StringComparer.Ordinal;

                        // Comprobamos que la contraseña recibida y la de la base de datos coincidan
                        if (comparador.Compare(jugador.contraseña, contraseña) == 0)
                        {
                            // La contraseña es correcta
                            error = 0;
                        }
                        else
                        {
                            // La contraseña es incorrecta
                            error = 2;
                        }
                    }
                    else
                    {
                        // No existe el usuario para esa partida
                        error = 1;
                    }
                    
                }
            }
            catch
            {
                // Error desconocido
                error = 3;
            }

            // Devolvemos el codigo de error
            return error;
        }

        // Metodo para obtener los parametros de una partida a partir de su id
        public infoPartidaEntity obtenerParametrosPartida(int id_partida)
        {
            // Creamos el objeto para almacenar las datos de la partida
            infoPartidaEntity partidaObtenida = null;

            try
            {

                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos la informacion de la partida de la base de datos
                    var partida = (from register in context.Partida
                                   where register.activo == true && 
                                   register.id_Partida == id_partida
                                   select new
                                   {
                                       register.Modalidad,
                                       register.Velocidad,
                                       register.Duracion,
                                       register.limiteJugadores,
                                       register.limitePoblacion,
                                       register.fechaInicio,
                                       jugadoresActivos = (from register2 in context.Jugador
                                                           where register2.id_Partida == register.id_Partida
                                                           select register2.nombreUsuario).Count()
                                   }).FirstOrDefault();

                    // Si la partida exite
                    if (partida != null)
                    {
                        // Almacenamos la informacion en el objeto que devolveremos
                        partidaObtenida = new infoPartidaEntity();

                        partidaObtenida.modalidad = partida.Modalidad;
                        partidaObtenida.velocidad = partida.Velocidad;
                        partidaObtenida.duracion = partida.Duracion;
                        partidaObtenida.limiteJugadores = partida.limiteJugadores;
                        partidaObtenida.limitePoblacion = partida.limitePoblacion;
                        partidaObtenida.fechaInicio = partida.fechaInicio;
                        partidaObtenida.jugadoresActivos = partida.jugadoresActivos;
                    }
                    
                }
            }
            catch
            {

            }

            // Devolvemos la informacion de la partida, si no existiera la partida devolvemos null
            return partidaObtenida;
        }

        // Metodo que realiza todas las acciones necesarias para acabar una partida y devuelve el jugador ganador
        public potenciaJugadorEntity comprobarFinalizacion(int id_Partida)
        {
            // Creamos el objeto que devolveremos con el jugador ganador
            potenciaJugadorEntity jugadorGanador = null;

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Creamos un objeto del servicio de manejo de jugador para llamar a sus metodos
                    servicioManejoJugadores objJugadores = new servicioManejoJugadores();


                    // obtenemos el registro de la partida
                    var partida = (from register in context.Partida
                                   where register.id_Partida == id_Partida
                                   select register).FirstOrDefault();

                    // Comprobamos si la partida ha acabado
                    if (partida.activo == false)
                    {
                        // Inicializamos el objeto de jugadorGanador
                        jugadorGanador = new potenciaJugadorEntity();

                        // obtenemos el jugador con mayor clasificacion
                        jugadorGanador = objJugadores.obtenerClasificacion(id_Partida)[0];

                        // Borramos los jugadores de la partida
                        objJugadores.borrarJugadores(id_Partida); // No funciona por que no hay borrado en cascada

                        // Confirmamos cambios
                        context.SaveChanges();
                    }                   
                }
            }
            catch
            {

            }
            
            // Devolvemos el jugador ganador
            return jugadorGanador;
        }
    }
}