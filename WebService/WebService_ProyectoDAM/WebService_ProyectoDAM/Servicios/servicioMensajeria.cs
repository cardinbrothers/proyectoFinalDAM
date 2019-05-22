using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_ProyectoDAM.Models;
using WebService_ProyectoDAM.ApiEntities;

namespace WebService_ProyectoDAM.Servicios
{
    public class servicioMensajeria
    {
        // Metodo para mandar un mensaje nuevo a un usuario
        public int mandarMensajenuevo(mensajesEntity mensaje)
        {
            // Valor entero que almacena un digito segun que error ocurra siendo: 
            //      0 --> Todo correcto
            //      1 --> Usuuario destinatario no existente
            //      2 --> Asunto vacio
            //      3 --> Contenido vacio
            //      4 --> Error desconocido
            int error = 0;

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos de la base de datos el usuario receptor del mensaje
                    var existeUsuario = (from register in context.Jugador
                                         where register.nombreUsuario == mensaje.usuarioReceptor
                                         select register).FirstOrDefault();

                    // Comprobamos si el usuario receptor exsite
                    if (existeUsuario != null)
                    {
                        // Comprobamos que haya un asunto
                        if (mensaje.asunto != null)
                        {
                            // Comprobamos que haya un contenido del mensaje
                            if (mensaje.contenido != null)
                            {
                                // Creamos el mensaje que almacenaremos en la base de datos
                                Mensaje mensajeNuevo = new Mensaje();
                                mensajeNuevo.usuarioEmisor = mensaje.usuarioEmisor;
                                mensajeNuevo.usuarioReceptor = mensaje.usuarioReceptor;
                                mensajeNuevo.fecha = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Europe Standard Time");
                                mensajeNuevo.asunto = mensaje.asunto;
                                mensajeNuevo.contenido = mensaje.contenido;

                                // Almacenamos el mensaje en la base de datos
                                context.Mensaje.Add(mensajeNuevo);
                                context.SaveChanges();
                            }
                            else
                            {
                                // No hay contenido de mensaje
                                error = 3;
                            }
                        }
                        else
                        {
                            // No hay un asunto para el mensaje
                            error = 2;
                        }
                    }
                    else
                    {
                        // El usuario receptor no existe
                        error = 1;
                    }
                }
            }
            catch
            {
                error = 4;
            }

            return error;
        }

        // Obtenemos los mensajes principales recibidos o enviados por el jugador indicado
        public List<mensajesEntity> listaMensajesPrinciapales(string nombreUsuario)
        {
            // Creamos la lista de mensajes que devolveremos
            List<mensajesEntity> bandejaMensajesPrincipales = new List<mensajesEntity>();

            try
            {
              using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos los mensajes principales del jugador
                    var listaPrincipales = from register in context.Mensaje
                                           where (register.usuarioEmisor == nombreUsuario || register.usuarioReceptor == nombreUsuario)
                                           && register.mensajePadre == register.id_Mensaje 
                                           select register;

                    // Tratamos cada mensaje con un foreach
                    foreach (var mensajePrincipal in listaPrincipales)
                    {
                        // Introducimos la informacion del mensaje en un objeto auxiliar
                        mensajesEntity mensajeAux = new mensajesEntity();
                        mensajeAux.id_Mensaje = mensajePrincipal.id_Mensaje;
                        mensajeAux.usuarioReceptor = mensajePrincipal.usuarioReceptor;
                        mensajeAux.usuarioEmisor = mensajePrincipal.usuarioEmisor;
                        mensajeAux.fecha = mensajePrincipal.fecha;
                        mensajeAux.contenido = mensajePrincipal.contenido;
                        mensajeAux.asunto = mensajePrincipal.asunto;
                        mensajeAux.mensajePadre = mensajePrincipal.mensajePadre; 

                        // Añadimos el objeto auxiliar a la lista de mensajes
                        bandejaMensajesPrincipales.Add(mensajeAux);

                    }
                }
            }
            catch
            {

            }

            // Devolvemos la lista de mensajes
            return bandejaMensajesPrincipales;
        }

        // Obtenemos los mensajes secundarios de un mensaje indicado
        public List<mensajesEntity> listaMensajesSecundarios(int id_Mensaje)
        {
            // Creamos la lista de mensajes que devolveremos
            List<mensajesEntity> bandejaMensajesSecundarios = new List<mensajesEntity>();

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos los mensajes secundarios del id_mensaje recibido
                    var listaSecundarios = from register in context.Mensaje
                                           where true
                                           // && register.dependido == id_Mensaje  --> Lo de actualizar la base de datos
                                           select register;

                    // Tratamos cada mensaje con un foreach
                    foreach (var mensajePrincipal in listaSecundarios)
                    {
                        // Introducimos la informacion del mensaje en un objeto auxiliar
                        mensajesEntity mensajeAux = new mensajesEntity();
                        mensajeAux.id_Mensaje = mensajePrincipal.id_Mensaje;
                        mensajeAux.usuarioReceptor = mensajePrincipal.usuarioReceptor;
                        mensajeAux.usuarioEmisor = mensajePrincipal.usuarioEmisor;
                        mensajeAux.fecha = mensajePrincipal.fecha;
                        mensajeAux.contenido = mensajePrincipal.contenido;
                        mensajeAux.asunto = mensajePrincipal.asunto;
                        mensajeAux.mensajePadre = mensajePrincipal.mensajePadre;

                        // Añadimos el objeto auxiliar a la lista de mensajes
                        bandejaMensajesSecundarios.Add(mensajeAux);

                    }
                }
            }
            catch
            {

            }

            // Devolvemos la lista de mensajes
            return bandejaMensajesSecundarios;
        }

        // Metodo para mandar responder a un hilo de mensajes
        public int responderMensaje(mensajesEntity mensaje)
        {
            // Valor entero que almacena un digito segun que error ocurra siendo: 
            //      0 --> Todo correcto
            //      1 --> Contenido vacio
            //      2 --> Error desconocido
            int error = 0;

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Comprobamos que haya un contenido del mensaje
                    if (mensaje.contenido != null)
                    {
                        // Creamos el mensaje que almacenaremos en la base de datos
                        Mensaje mensajeNuevo = new Mensaje();
                        mensajeNuevo.usuarioEmisor = mensaje.usuarioEmisor;
                        mensajeNuevo.usuarioReceptor = mensaje.usuarioReceptor;
                        mensajeNuevo.fecha = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Europe Standard Time");
                        mensajeNuevo.contenido = mensaje.contenido;
                        mensajeNuevo.mensajePadre = mensaje.mensajePadre;

                        // Almacenamos el mensaje en la base de datos
                        context.Mensaje.Add(mensajeNuevo);
                        context.SaveChanges();
                    }
                    else
                    {
                        // No tiene contenido el mensaje
                        error = 1;
                    }
                }
            }
            catch
            {
                // Error desconocido
                error = 2;
            }

            // Devolvemos el codigo de error
            return error;
        }

    }
}