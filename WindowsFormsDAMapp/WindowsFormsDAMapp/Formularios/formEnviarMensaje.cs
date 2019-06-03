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
        List<puebloEntity> listaPueblos;
        bool SalidaForm = false;

        public formEnviarMensaje(sessionInfo info)
        {
            infoSesion = info;
            InitializeComponent();
        }

        private void FormEnviarMensaje_Load(object sender, EventArgs e)
        {
            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);
            
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            this.Location = infoSesion.posicionFormulario;

            this.Text = "Jugador: " + infoSesion.nombreUsuario;

        }

        private void Btn_enviar_Click(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

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
            if (!SalidaForm)
            {

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

        private void formEnviarMensaje_FormClosing(object sender, FormClosingEventArgs e)
        {
            SalidaForm = true;
        }

        private void FormEnviarMensaje_LocationChanged(object sender, EventArgs e)
        {
            formEnviarMensaje frmAux = (formEnviarMensaje)sender;
            Point position = frmAux.Location;
            infoSesion.posicionFormulario = position;
        }
    }
    
}
