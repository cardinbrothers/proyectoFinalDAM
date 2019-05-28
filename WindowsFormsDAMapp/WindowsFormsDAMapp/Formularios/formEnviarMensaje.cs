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
    public partial class formEnviarMensaje : Form
    {
        sessionInfo infoSesion; 
        RestClient restClient = new RestClient();
        webServiceInfo session = new webServiceInfo();

        public formEnviarMensaje(sessionInfo info)
        {
            infoSesion = info;
            InitializeComponent();
        }

        private void FormEnviarMensaje_Load(object sender, EventArgs e)
        {
            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);

        }

        private void Btn_enviar_Click(object sender, EventArgs e)
        {
            mensajesEntity mensajeEnviado = new mensajesEntity();

            mensajeEnviado.asunto = tbx_asunto.Text;
            mensajeEnviado.usuarioEmisor = infoSesion.nombreUsuario;
            mensajeEnviado.usuarioReceptor = tbx_jugador.Text;
            mensajeEnviado.contenido = tbx_contenidoMensaje.Text;

            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Mensaje/mandarMensaje", Method.POST);

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
                    tbx_asunto.Clear();
                    tbx_contenidoMensaje.Clear();
                    tbx_jugador.Clear();
                    break;
                case 1:
                    MessageBox.Show("El jugador introducido no exite");
                    break;
                case 2:
                    MessageBox.Show("El asunto del mensaje no puede ir vacío");
                    break;
                case 3:
                    MessageBox.Show("El contenido del mensaje no puede ir vacío");
                    break;
                case 4:
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

        private void btn_mapa_Click(object sender, EventArgs e)
        {
            // Creamos un objeto del formulario de inicio de sesion
            frmMapa frm_mapa = new frmMapa(infoSesion);

            // Lanzamos el objeto de inicio de sesion   
            frm_mapa.Show();

            // Cerramos este formulario
            this.Close();
        }

        private void btn_movimientos_Click(object sender, EventArgs e)
        {
            // Creamos un objeto del formulario de inicio de sesion
            formMovimientos frm_movimientos = new formMovimientos(infoSesion);

            // Lanzamos el objeto de inicio de sesion   
            frm_movimientos.Show();

            // Cerramos este formulario
            this.Close();
        }
    }
    
}
