using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsDAMapp.Helpers;
using WindowsFormsDAMapp.Entidades;
using RestSharp;
using Newtonsoft.Json;

namespace WindowsFormsDAMapp
{
    public partial class formContestarMensaje : Form
    {
        sessionInfo infoSesion;
        RestClient restClient = new RestClient();
        webServiceInfo session = new webServiceInfo();
        List<mensajesEntity> listaMensajes;
        mensajesEntity mensajePadre;
        List<puebloEntity> listaPueblos;
        bool SalidaForm = false;

        public formContestarMensaje(mensajesEntity mensajePadre, sessionInfo info)
        {
            this.mensajePadre = mensajePadre;
            infoSesion = info;
            InitializeComponent();
        }

        private void FormContestarMensaje_Load(object sender, EventArgs e)
        {
            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);
            
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();                    

            // Obtenemos la lista de mensajes
            listaMensajes = obtenerMensajes();

            // Introducimos los mensajes en el listView
            mostrarMensajes(listaMensajes);
        }

        private List<mensajesEntity> obtenerMensajes()
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Mensaje/obtenerMensajesS", Method.GET);

            // Añadimos el nombre de usuario a la peticion
            peticion.AddParameter("id_Mensaje", mensajePadre.id_Mensaje);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            List<mensajesEntity> result = JsonConvert.DeserializeObject<List<mensajesEntity>>(response.Content);

            return result;
        }

        private void mostrarMensajes(List<mensajesEntity> mensajes)
        {
            tbx_contenidoMensaje.Clear();
            string cadenaMensajes = "";

            foreach (var mensaje in mensajes)
            {
                // Comprobamos si el jugador es el receptor o emisor del mensaje 
                if (mensaje.usuarioReceptor == infoSesion.nombreUsuario)
                {
                    // Introducimos el autor + 2 saltos de linea
                    cadenaMensajes += mensaje.usuarioEmisor + " escribió en el día " + mensaje.fecha.ToShortDateString() + Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    // Introducimos el autor + 2 saltos de linea
                    cadenaMensajes += "Tu escribiste en el día " + mensaje.fecha.ToShortDateString() + Environment.NewLine + Environment.NewLine;
                }

                // Introducimos el contenid + 3 saltos de linea
                cadenaMensajes += mensaje.contenido + Environment.NewLine + Environment.NewLine + Environment.NewLine;

            }

            tbx_contenidoMensaje.Text = cadenaMensajes;
        }

        private void Btn_responder_Click(object sender, EventArgs e)
        {
            // Hacemos visibles unos controles e invisibles otros según corresponda
            tbx_respuesta.Visible = true;
            btn_aceptar.Visible = true;
            btn_cancelar.Visible = true;
            btn_responder.Visible = false;

        }

        private void Btn_cancelar_Click(object sender, EventArgs e)
        {
            // Hacemos visibles unos controles e invisibles otros según corresponda
            tbx_respuesta.Visible = false;
            btn_aceptar.Visible = false;
            btn_cancelar.Visible = false;
            btn_responder.Visible = true;
            tbx_respuesta.Clear();

            // Obtenemos la lista de mensajes
            listaMensajes = obtenerMensajes();

            // Introducimos los mensajes en el listView
            mostrarMensajes(listaMensajes);
        }

        private void Btn_aceptar_Click(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            mensajesEntity mensajeEnviado = new mensajesEntity();

            mensajeEnviado.usuarioEmisor = infoSesion.nombreUsuario;
            // Comprobamos si el jugador es el receptor o emisor del mensaje padre
            if (mensajePadre.usuarioReceptor == infoSesion.nombreUsuario)
            {
                mensajeEnviado.usuarioReceptor = mensajePadre.usuarioEmisor;
            }
            else
            {
                mensajeEnviado.usuarioReceptor = mensajePadre.usuarioReceptor;
            }
            mensajeEnviado.contenido = tbx_respuesta.Text;
            mensajeEnviado.mensajePadre = mensajePadre.id_Mensaje;
            mensajeEnviado.asunto = mensajePadre.asunto;

            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Mensaje/responderMensaje", Method.POST);

            // Añadimos la informacion del usuario nuevo a la peticion
            peticion.AddJsonBody(mensajeEnviado);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo en un int
            int result = JsonConvert.DeserializeObject<int>(response.Content);

            switch (result)
            {
                case 0:
                    MessageBox.Show("Mensaje Enviado correctamente");
                    tbx_respuesta.Visible = false;
                    btn_aceptar.Visible = false;
                    btn_cancelar.Visible = false;
                    btn_responder.Visible = true;
                    tbx_respuesta.Clear();

                    // Obtenemos la lista de mensajes
                    listaMensajes = obtenerMensajes();

                    // Introducimos los mensajes en el listView
                    mostrarMensajes(listaMensajes);
                    break;
                case 1:
                    MessageBox.Show("El contenido del mensaje no puede ir vacío");
                    break;
                case 2:
                    MessageBox.Show("Error desconocido");
                    break;
            }
        }

        private void Btn_volver_Click(object sender, EventArgs e)
        {
            frm_MenuPrincipal formularioPrincipal = new frm_MenuPrincipal();

            formularioPrincipal.Show();

            this.Close();
        }

        private void Btn_visionGeneral_Click(object sender, EventArgs e)
        { 
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            try
            {
                if (!SalidaForm)
                {
                    // Comprobamos que el pueblo seleccionado sigue perteneciendo al jugador
                    if (listaPueblos.FindAll(x => x.id_Pueblo == infoSesion.id_Pueblo).FirstOrDefault() == null)
                    {
                        infoSesion.id_Pueblo = listaPueblos[0].id_Pueblo;

                    }

                    // Creamos un objeto del formulario de inicio de sesion
                    formVisionGeneral VisionGeneral = new formVisionGeneral(infoSesion);

                    // Lanzamos el objeto de inicio de sesion   
                    VisionGeneral.Show();

                    // Cerramos este formulario
                    this.Close();
                }
            }
            catch
            {

            }
        }

        private void Btn_reclutamiento_Click(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            try
            {
                if (!SalidaForm)
                {
                    // Comprobamos que el pueblo seleccionado sigue perteneciendo al jugador
                    if (listaPueblos.FindAll(x => x.id_Pueblo == infoSesion.id_Pueblo).FirstOrDefault() == null)
                    {
                        infoSesion.id_Pueblo = listaPueblos[0].id_Pueblo;

                    }

                    // Creamos un objeto del formulario de reclutamiento
                    formReclutamiento reclutamiento = new formReclutamiento(infoSesion);

                    // Lanzamos el formulario de reclutamiento
                    reclutamiento.Show();

                    // Cerramos este formulario
                    this.Close();
                }
            }
            catch
            {

            }
        }

        private void Btn_Clasificacion_Click(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            try
            {
                if (!SalidaForm)
                {
                    // Comprobamos que el pueblo seleccionado sigue perteneciendo al jugador
                    if (listaPueblos.FindAll(x => x.id_Pueblo == infoSesion.id_Pueblo).FirstOrDefault() == null)
                    {
                        infoSesion.id_Pueblo = listaPueblos[0].id_Pueblo;
                    }

                    // Creamos un objeto del formulario de reclutamiento
                    formClasificacion clasificacion = new formClasificacion(infoSesion);

                    // Lanzamos el formulario de reclutamiento
                    clasificacion.Show();

                    // Cerramos este formulario
                    this.Close();
                }
            }
            catch
            {

            }
        }

        private void Btn_mensajes_Click(object sender, EventArgs e)
        {

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            try
            {
                if (!SalidaForm)
                {
                    // Comprobamos que el pueblo seleccionado sigue perteneciendo al jugador
                    if (listaPueblos.FindAll(x => x.id_Pueblo == infoSesion.id_Pueblo).FirstOrDefault() == null)
                    {
                        infoSesion.id_Pueblo = listaPueblos[0].id_Pueblo;
                    }
                    // Creamos un objeto del formulario de reclutamiento
                    formBandejaEntrada bandejaEntrada = new formBandejaEntrada(infoSesion);

                    // Lanzamos el formulario de reclutamiento
                    bandejaEntrada.Show();

                    // Cerramos este formulario
                    this.Close();
                }
            }
            catch
            {

            }
        }

        private void btn_mapa_Click(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            try
            {
                if (!SalidaForm)
                {
                    // Comprobamos que el pueblo seleccionado sigue perteneciendo al jugador
                    if (listaPueblos.FindAll(x => x.id_Pueblo == infoSesion.id_Pueblo).FirstOrDefault() == null)
                    {
                        infoSesion.id_Pueblo = listaPueblos[0].id_Pueblo;
                    }

                    // Creamos un objeto del formulario de inicio de sesion
                    frmMapa frm_mapa = new frmMapa(infoSesion);

                    // Lanzamos el objeto de inicio de sesion   
                    frm_mapa.Show();

                    // Cerramos este formulario
                    this.Close();
                }
            }
            catch
            {

            }
        }

        private void btn_movimientos_Click(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            try
            {
                if (!SalidaForm)
                {
                    // Comprobamos que el pueblo seleccionado sigue perteneciendo al jugador
                    if (listaPueblos.FindAll(x => x.id_Pueblo == infoSesion.id_Pueblo).FirstOrDefault() == null)
                    {
                        infoSesion.id_Pueblo = listaPueblos[0].id_Pueblo;
                    }

                    // Creamos un objeto del formulario de inicio de sesion
                    formMovimientos frm_Movimientos = new formMovimientos(infoSesion);

                    // Lanzamos el objeto de inicio de sesion   
                    frm_Movimientos.Show();

                    // Cerramos este formulario
                    this.Close();
                }
            }
            catch
            {

            }
        }


        // Comprobar si ha finalizado la partida
        private void comprobarFinPartida()
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Partida/comprobarFinallizacion", Method.GET);

            // Añadimos el id de la partida a la peticion
            peticion.AddParameter("id_Partida", infoSesion.id_partida);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            potenciaJugadorEntity result = JsonConvert.DeserializeObject<potenciaJugadorEntity>(response.Content);

            if (result != null)
            {
                var userResponse = MessageBox.Show("La partida ha finalizado! el ganador es: " + result.nombreJugador + "!!!");
                Btn_volver_Click(null, null);

            }
        }

        private List<puebloEntity> obtenerListaPueblos(string nombreUsuario)
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Pueblo/obtenerListaPueblos", Method.GET);

            // Añadimos el nombre del usuario a la peticion
            peticion.AddParameter("propietario", nombreUsuario);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            List<puebloEntity> result = JsonConvert.DeserializeObject<List<puebloEntity>>(response.Content);

            return result;

        }

        // Comprobar si nos han quitado todos los pueblos
        private void comprobarPosesionPueblos()
        {
            if (!SalidaForm)
            {
                // Obtenemos los pueblos del jugador
                listaPueblos = obtenerListaPueblos(infoSesion.nombreUsuario);

                if (listaPueblos.Count <= 0 || listaPueblos == null)
                {
                    var userResponse = MessageBox.Show("Te han quitado todos los pueblos, perdiste la partida.");
                    Btn_volver_Click(null, null);
                }
            }
        }

        private void formContestarMensaje_FormClosing(object sender, FormClosingEventArgs e)
        {
            SalidaForm = true;
        }
    }
}
