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
    public partial class formClasificacion : Form
    {
        sessionInfo infoSesion;
        RestClient restClient = new RestClient();
        webServiceInfo session = new webServiceInfo();
        List<potenciaJugadorEntity> listaClasificacion;
        List<puebloEntity> listaPueblos;
        bool SalidaForm = false;

        public formClasificacion(sessionInfo infoSesion)
        {
            this.infoSesion = infoSesion;
            InitializeComponent();
        }

        private void FormClasificacion_Load(object sender, EventArgs e)
        {
            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();
            
            // Obtenemos la clasificacion de la partida
            listaClasificacion = obtenerClasificacion();

            // Mostramos la clasificacion de la partida
            mostrarClasificacion(listaClasificacion);


        }

        // Obtenemos la clasificacion de la partida
        private List<potenciaJugadorEntity> obtenerClasificacion()
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Jugadores/obtenerClasificacion", Method.GET);

            // Añadimos el id de la partida a la peticion
            peticion.AddParameter("id_Partida", infoSesion.id_partida);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            List<potenciaJugadorEntity> result = JsonConvert.DeserializeObject<List<potenciaJugadorEntity>>(response.Content);

            result = result.OrderByDescending(x => x.potenciaJugador).ToList();

            return result;
        }

        // Metodo para visualizar la clasificacione en el listView
        private void mostrarClasificacion(List<potenciaJugadorEntity> clasificacion)
        {
            lsv_clasificacion.Items.Clear();


            foreach (var jugador in clasificacion)
            {
                // Introducimos la informacion en el listview
                ListViewItem itemAux = new ListViewItem(jugador.nombreJugador);
                itemAux.SubItems.Add(obtenerNumeroPueblos(jugador.nombreJugador).ToString());
                itemAux.SubItems.Add(jugador.potenciaJugador.ToString());

                lsv_clasificacion.Items.Add(itemAux);
            }
        }

        private int obtenerNumeroPueblos(string nombreUsuario)
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Pueblo/obtenerListaPueblos", Method.GET);

            // Añadimos el nombre del usuario a la peticion
            peticion.AddParameter("propietario", nombreUsuario);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            List<puebloEntity> result = JsonConvert.DeserializeObject<List<puebloEntity>>(response.Content);

            // Devolvemos el numero de elementos en la lista
            return result.Count;

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

        // Metodo para volver al menu principal
        private void Btn_volver_Click(object sender, EventArgs e)
        {
            frm_MenuPrincipal formularioPrincipal = new frm_MenuPrincipal();

            formularioPrincipal.Show();

            this.Close();
        }

        // Metodo asociado a la recarga de la vista
        private void Btn_Clasificacion_Click(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            // Obtenemos la clasificacion de la partida
            listaClasificacion = obtenerClasificacion();

            // Mostramos la clasificacion de la partida
            mostrarClasificacion(listaClasificacion);
        }

        private void Btn_mensajes_Click(object sender, EventArgs e)
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
                formBandejaEntrada bandejaEntrada = new formBandejaEntrada(infoSesion);

                // Lanzamos el formulario de reclutamiento
                bandejaEntrada.Show();

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

        private void formClasificacion_FormClosing(object sender, FormClosingEventArgs e)
        {
            SalidaForm = true;
        }
    }
}
