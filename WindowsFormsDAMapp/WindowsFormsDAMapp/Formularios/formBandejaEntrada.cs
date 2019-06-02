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
    public partial class formBandejaEntrada : Form
    {
        sessionInfo infoSesion;
        RestClient restClient = new RestClient();
        webServiceInfo session = new webServiceInfo();
        List<mensajesEntity> listaMensajes;
        List<puebloEntity> listaPueblos;


        public formBandejaEntrada(sessionInfo info)
        {
            infoSesion = info;
            InitializeComponent();
        }

        private void FormBandejaEntrada_Load(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

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
            RestRequest peticion = new RestRequest("/api/Mensaje/obtenerMensajesP", Method.GET);

            // Añadimos el nombre de usuario a la peticion
            peticion.AddParameter("nombreUsuario", infoSesion.nombreUsuario);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            List<mensajesEntity> result = JsonConvert.DeserializeObject<List<mensajesEntity>>(response.Content);

            return result;
        }

        private void mostrarMensajes(List<mensajesEntity> mensajes)
        {
            lsv_mensajes.Items.Clear();

            foreach (var mensaje in mensajes)
            {
                ListViewItem itemAux = new ListViewItem(mensaje.asunto);

                // Comprobamos si el jugador es el receptor o emisor del mensaje para mostrar siempre el nombre del otro jugador y no uno mismo
                if (mensaje.usuarioReceptor == infoSesion.nombreUsuario)
                {
                    itemAux.SubItems.Add(mensaje.usuarioEmisor);
                }
                else
                {
                    itemAux.SubItems.Add(mensaje.usuarioReceptor);
                }

                itemAux.SubItems.Add(mensaje.fecha.ToString("dd/MM/yyyy"));

                // Añadimos el mensaje al listview
                lsv_mensajes.Items.Add(itemAux);
            }
        }

        private void Btn_visionGeneral_Click(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            try
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

            // Obtenemos la lista de mensajes
            listaMensajes = obtenerMensajes();

            // Introducimos los mensajes en el listView
            mostrarMensajes(listaMensajes);
        }

        private void Btn_volver_Click(object sender, EventArgs e)
        {
            frm_MenuPrincipal formularioPrincipal = new frm_MenuPrincipal();

            formularioPrincipal.Show();

            this.Close();
        }

        private void Btn_redactar_Click(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            try
            {
                // Comprobamos que el pueblo seleccionado sigue perteneciendo al jugador
                if (listaPueblos.FindAll(x => x.id_Pueblo == infoSesion.id_Pueblo).FirstOrDefault() == null)
                {
                    infoSesion.id_Pueblo = listaPueblos[0].id_Pueblo;
                }

                formEnviarMensaje enviarMensaje = new formEnviarMensaje(infoSesion);

                enviarMensaje.Show();

                this.Close();
            }
            catch
            {

            }
        }

        private void Lsv_mensajes_DoubleClick(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            try
            {
                // Comprobamos que el pueblo seleccionado sigue perteneciendo al jugador
                if (listaPueblos.FindAll(x => x.id_Pueblo == infoSesion.id_Pueblo).FirstOrDefault() == null)
                {
                    infoSesion.id_Pueblo = listaPueblos[0].id_Pueblo;
                }

                mensajesEntity mensajePadre;

                // Obtenemos el mensaje padre
                mensajePadre = listaMensajes[lsv_mensajes.SelectedIndices[0]];

                // Creamos un objeto del formulario contestar mensaje
                formContestarMensaje responderMensaje = new formContestarMensaje(mensajePadre, infoSesion);

                // Lanzamos el formulario
                responderMensaje.Show();

                // Cerramos este formulario
                this.Close();
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
            // Obtenemos los pueblos del jugador
            listaPueblos = obtenerListaPueblos(infoSesion.nombreUsuario);

            if (listaPueblos.Count <= 0)
            {
                var userResponse = MessageBox.Show("Te han quitado todos los pueblos, perdiste la partida.");
                Btn_volver_Click(null, null);
            }
        }
    }
}
