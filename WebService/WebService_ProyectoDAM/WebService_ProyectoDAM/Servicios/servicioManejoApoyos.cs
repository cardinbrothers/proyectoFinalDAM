using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService_ProyectoDAM.Models;
using WebService_ProyectoDAM.ApiEntities;

namespace WebService_ProyectoDAM.Servicios
{
    public class servicioManejoApoyos
    {
        // Metodo que realiza las acciones necesarias cuando llega un apoyo a un pueblo y devuelve la informacion de este 
        public apoyosEntity llegadaApoyo(int id_Movimiento)
        {
            // Creamos el objeto donde almacenaremos la informacion del apoyo
            apoyosEntity apoyoCreado = new apoyosEntity();

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos el movimiento de tropas que ha finalizado
                    var movimiento = (from register in context.Movimientos
                                      where register.id_Movimiento == id_Movimiento &&
                                      register.tipoMovimiento == "apoyo" &&
                                      register.vencedor == -1
                                      select new
                                      {
                                          register.puebloOrigen,
                                          register.puebloDestino,
                                          register.horaLlegada,
                                          register.arqueros,
                                          register.ballesteros
                                      }).FirstOrDefault();

                    // Si el movimeinto existe
                    if (movimiento != null)
                    {
                        // Creamos un objeto de apoyos para insertarlo en la base de datos
                        Apoyos insertApoyo = new Apoyos();
                        insertApoyo.puebloOrigen = movimiento.puebloOrigen;
                        insertApoyo.puebloDestino = movimiento.puebloDestino;
                        insertApoyo.horaFin = movimiento.horaLlegada.AddMinutes(20);
                        insertApoyo.arqueros = movimiento.arqueros;
                        insertApoyo.ballesteros = movimiento.ballesteros;

                        // Insertamos el objeto de apoyos
                        context.Apoyos.Add(insertApoyo);

                        // Obtenemos el pueblo destino de los apoyos
                        var puebloDestino = (from register in context.Pueblo
                                            where register.id_Pueblo == movimiento.puebloDestino
                                            select register).FirstOrDefault();

                        // Cambiamos los valores de las tropas para que reflejen los apoyos en el pueblo
                        puebloDestino.arqueros += movimiento.arqueros;
                        puebloDestino.ballesteros += movimiento.ballesteros;

                        // Confirmamos los cambios
                        context.SaveChanges();

                        // Almacenamos los datos del apoyo en el objeto que devolvemos
                        apoyoCreado.id_Apoyo = insertApoyo.id_Apoyo;   // No se yo si esto funciona y ya contiene el id del apoyo eh !OJO!
                        apoyoCreado.puebloOrigen = insertApoyo.puebloOrigen;
                        apoyoCreado.puebloDestino = insertApoyo.puebloDestino;
                        apoyoCreado.ballesteros = (int)insertApoyo.ballesteros;
                        apoyoCreado.arqueros = (int)insertApoyo.arqueros;
                        apoyoCreado.horaFin = (DateTime)insertApoyo.horaFin;

                    }

                }
            }
            catch
            {

            }

            // Devolvemos el objeto
            return apoyoCreado;
        }

        // Metodo que gestiona la vuelta de un apoyo a su pueblo de origen cuando finaliza
        public int apoyoFinalizado(int id_Apoyo)
        {
            // Codigo de error que devolveremos 
            // 0 --> Todo correcto
            // 1 --> Apoyo no existente
            // 2 --> Error desconocido
            int error = 0; 

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos el registro de apoyo que hay que gestionar
                    var apoyo = (from register in context.Apoyos
                                where register.id_Apoyo == id_Apoyo
                                select register).FirstOrDefault();

                    // Comprobamos si el apoyo existe y ha finalizado ya
                    if (apoyo != null)
                    {
                        // Obtenemos el registro del pueblo de origen del apoyo
                        var puebloOrigen = (from register in context.Pueblo
                                            where register.id_Pueblo == apoyo.puebloOrigen
                                            select register).FirstOrDefault();

                        // Obtenemos el registro del pueblo destino del apoyo
                        var puebloDestino = (from register in context.Pueblo
                                            where register.id_Pueblo == apoyo.puebloDestino
                                            select register).FirstOrDefault();

                        // Modificamos los campos de arqueros y ballesteros en ambos pueblos
                        puebloOrigen.arqueros += apoyo.arqueros;
                        puebloOrigen.ballesteros += apoyo.ballesteros;
                        puebloDestino.arqueros -= apoyo.arqueros;
                        puebloDestino.ballesteros -= apoyo.ballesteros;

                        // Borramos el registro de apoyo de su tabla y confirmamos los cambios
                        context.Apoyos.Remove(apoyo);
                        context.SaveChanges();                 

                    }
                    else
                    {
                        // No existe un apoyo
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

        // Metodo que devuelve todos los apyos cuyo destino es el el pueblo indicado y devuelve aquellos que hayan acabado
        public List<apoyosEntity> obtenerApoyosActivos(int id_pueblo)
        {
            // Creamos la lista para devolver los apoyos del pueblo
            List<apoyosEntity> listaApoyosActivos = new List<apoyosEntity>();

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos todos los apoyos del pueblo
                    var listaApoyos = from register in context.Apoyos
                                      where register.puebloDestino == id_pueblo
                                      select register;

                    // Tratamos cada apoyo con un foreach
                    foreach (var apoyo in listaApoyos)
                    {
                        // Comprobamos si el apoyo ha pasado ya la fecha de finalizacion
                        if (apoyo.horaFin > DateTime.Now)
                        {
                            // Llamamamos al metodo para realizar las acciones correspondientes
                            apoyoFinalizado(apoyo.id_Apoyo);
                        }
                        else
                        {
                            // si no ha finalizado almacenamos sus datos en una obeto auxiliar
                            apoyosEntity apoyoAux = new apoyosEntity();
                            apoyoAux.id_Apoyo = apoyo.id_Apoyo;
                            apoyoAux.puebloOrigen = apoyo.puebloOrigen;
                            apoyoAux.puebloDestino = apoyo.puebloDestino;
                            apoyoAux.arqueros = (int)apoyo.arqueros;
                            apoyoAux.ballesteros = (int)apoyo.ballesteros;
                            apoyoAux.horaFin = (DateTime)apoyo.horaFin;

                            // Añadimos el objeto auxiliar a la lista que devolveremos
                            listaApoyosActivos.Add(apoyoAux);
                        }

                    }
                }
            }
            catch
            {

            }

            // Devolvemos la lista
            return listaApoyosActivos;
        }

        // Metodo que permite actualizar las tropas perdidas de un apoyo
        public int actualizarApoyo(int id_Apoyo, int cantArqueros, int cantBallesteros)
        {
            // Codigo de error que devolveremos 
            // 0 --> Todo correcto
            // 1 --> Apoyo no existe
            // 2 --> Error desconocido
            int error = 0;

            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // Obtenemos el apoyo cuyo id recibimos de la base de datos
                    var apoyo = (from register in context.Apoyos
                                 where register.id_Apoyo == id_Apoyo
                                 select register).FirstOrDefault();

                    // Conmprobamos si el apoyo existe
                    if (apoyo != null)
                    {
                        // Obtenemos el registro del pueblo de origen del apoyo
                        var puebloOrigen = (from register in context.Pueblo
                                            where register.id_Pueblo == apoyo.puebloOrigen
                                            select register).FirstOrDefault();

                        // Obtenemos el registro del pueblo destino del apoyo
                        var puebloDestino = (from register in context.Pueblo
                                            where register.id_Pueblo == apoyo.puebloDestino
                                            select register).FirstOrDefault();

                        var infoTropa = from register in context.Tropas
                                        orderby register.id_Tropas
                                        select new
                                        {
                                            register.poblacion,
                                            register.potencia
                                        };

                        var tropas = infoTropa.ToList();

                        // Modificamos la poblacion del pueblo origen
                        puebloOrigen.poblacion += cantArqueros * tropas[0].poblacion + cantArqueros * tropas[1].poblacion;

                        // Modificamos los arqueros y ballesteros del pueblo destino
                        puebloDestino.arqueros -= cantArqueros;
                        puebloDestino.ballesteros -= cantBallesteros;

                        // Modificamos los arqueros y ballesteros del apoyo
                        apoyo.ballesteros -= cantBallesteros;
                        apoyo.arqueros -= cantArqueros;

                        // Confirmamos los cambios
                        context.SaveChanges();
                    }
                    else
                    {
                        // El apoyo no existe
                        error = 1;
                    }
                }
            }
            catch
            {
                // Error desconocido
                error = 2;
            }

            // Devolvemos el codigo del error
            return error;
        }

        // Metodo que finaliza todos los apoyos que hayan superado su limite
        public void finalizarApoyosExpirados()
        {
            try
            {
                using (var context = new ProyectoDAMEntities())
                {
                    // obtenemos los apoyos que han superado su limite de tiempo
                    var apoyosFinalizados = from register in context.Apoyos
                                            where register.horaFin > DateTime.Now
                                            select register;

                    // Tratamos cada apoyo con un foreach
                    foreach (var apoyo in apoyosFinalizados)
                    {
                        // Llamamos al metodo que realiza la logica de finalizar un apoyo mandandole el id
                        apoyoFinalizado(apoyo.id_Apoyo);
                    }
                }
            }
            catch
            {

            }
        }
    }
}

// Metodos: Llegada apoyo; vuelta apoyo; obtener apoyos de un pueblo; actualizar apoyo; Finalizar todos los apoyos