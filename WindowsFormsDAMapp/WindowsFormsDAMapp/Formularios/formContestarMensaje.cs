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
                    cadenaMensajes += mensaje.usuarioEmisor + " escribió en el día " + mensaje.fecha.ToShortDateString() + ": \n\n";
                }
                else
                {
                    // Introducimos el autor + 2 saltos de linea
                    cadenaMensajes += "Tu escribiste en el día " + mensaje.fecha.ToShortDateString() + ": \n\n";
                }

                // Introducimos el contenid + 3 saltos de linea
                cadenaMensajes += mensaje.contenido + "\n\n\n";

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
            // Creamos un objeto del formulario de inicio de sesion
            formVisionGeneral VisionGeneral = new formVisionGeneral(infoSesion);

            // Lanzamos el objeto de inicio de sesion   
            VisionGeneral.Show();

            // Cerramos este formulario
            this.Close();
        }

        private void Btn_reclutamiento_Click(object sender, EventArgs e)
        {
            // Creamos un objeto del formulario de reclutamiento
            formReclutamiento reclutamiento = new formReclutamiento(infoSesion);

            // Lanzamos el formulario de reclutamiento
            reclutamiento.Show();

            // Cerramos este formulario
            this.Close();
        }

        private void Btn_Clasificacion_Click(object sender, EventArgs e)
        {
            // Creamos un objeto del formulario de reclutamiento
            formClasificacion clasificacion = new formClasificacion(infoSesion);

            // Lanzamos el formulario de reclutamiento
            clasificacion.Show();

            // Cerramos este formulario
            this.Close();
        }

        private void Btn_mensajes_Click(object sender, EventArgs e)
        {
            // Creamos un objeto del formulario de reclutamiento
            formBandejaEntrada bandejaEntrada = new formBandejaEntrada(infoSesion);

            // Lanzamos el formulario de reclutamiento
            bandejaEntrada.Show();

            // Cerramos este formulario
            this.Close();
        }
    }
}
