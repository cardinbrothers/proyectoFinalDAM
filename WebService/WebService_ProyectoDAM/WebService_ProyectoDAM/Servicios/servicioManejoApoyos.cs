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
                        apoyoCreado.id_Apoyo = insertApoyo.id_Apoyo;
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
    }
}

// Metodos: Llegada apoyo; vuelta apoyo; obtener apoyos de un pueblo; actualizar apoyo