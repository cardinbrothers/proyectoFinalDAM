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
    public partial class frmMapa : Form
    {

        sessionInfo infoSesion;
        RestClient restClient = new RestClient();
        webServiceInfo session = new webServiceInfo();
        List<puebloEntity> listaPueblos;
        List<string> listaCoordsPartida;

        public frmMapa(sessionInfo infoSesion)
        {
            this.infoSesion = infoSesion;
            InitializeComponent();
        }

        private void Mapa_Load(object sender, EventArgs e)
        {

            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            // Introducimos los pueblos en el comboBox
            cbx_pueblos.ValueMember = "id_Pueblo";
            cbx_pueblos.DisplayMember = "coordenadas";
            cbx_pueblos.DataSource = listaPueblos;

            // Seleccionamos el pueblo deseado
            cbx_pueblos.SelectedValue = infoSesion.id_Pueblo;

            // obtenemos todos las coords
            listaCoordsPartida = obtenerCoordsPartida(infoSesion.id_partida);

            //Pintamos los pueblos
            pintarCoordenadas();
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

        private List<string> obtenerCoordsPartida(int id_Partida)
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Pueblo/obtenerCoords", Method.GET);

            // Añadimos el id de la partida a la peticion
            peticion.AddParameter("id_Partida", id_Partida);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            List<string> result = JsonConvert.DeserializeObject<List<string>>(response.Content);

            return result;
        }

        // Metodo para pintar las coordenadas en el mapa
        private void pintarCoordenadas()
        {
            // Obtenemos las coordenadas del pueblo actual
            string coordActual = listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault().coordenadas;

            List<string> coordsPropias = new List<string>();

            //Obtenemos las coordenadas de todos los pueblos propios
            foreach (var pueblo in listaPueblos)
            {
                coordsPropias.Add(pueblo.coordenadas);
            }
            
            // Recorremos todos los pictureBox
            foreach (var pbx in this.Controls)
            {
                if (pbx is PictureBox)
                {
                    PictureBox pbxAux = (PictureBox)pbx;
                    pbxAux.Enabled = true;


                    // Comprobamos si la coordenada es del pueblo actual, de uno propio o de un enemigo para pintar de diferentes colores segun corresponda
                    if ((string)pbxAux.Tag == coordActual)
                    {
                        // Si es el pueblo actual pintamos de azul oscuro
                        pbxAux.Image = Properties.Resources.punto_azul_oscuro;
                    }
                    else
                    {
                        if (coordsPropias.Contains((string)pbxAux.Tag))
                        {
                            // Si es un pueblo propio pintamos de azul claro
                            pbxAux.Image = Properties.Resources.punto_azul_png;

                        }
                        else
                        {
                            if (listaCoordsPartida.Contains((string)pbxAux.Tag))
                            {
                                // Si es el pueblo es enemigo pintamos de rojo
                                pbxAux.Image = Properties.Resources.punto_rojo;
                            }
                            else
                            {
                                // Deshabilitamos el picture box si no tiene pueblo asignado
                                pbxAux.Enabled = false;
                            }
                        }

                    }
                }
            }
                
            
        }

        private void btn_volver_Click(object sender, EventArgs e)
        {
            frm_MenuPrincipal formularioPrincipal = new frm_MenuPrincipal();

            formularioPrincipal.Show();

            this.Close();
        }

        private void cbx_pueblos_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();


            // Obtenemos el pueblo seleccionado
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            if (listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault() != null)
            {

                // Introducimos los pueblos en el comboBox
                cbx_pueblos.DataSource = listaPueblos;

               
                // Seleccionamos el pueblo anterior
                cbx_pueblos.SelectedValue = infoSesion.id_Pueblo;
                

                // obtenemos todos las coords
                listaCoordsPartida = obtenerCoordsPartida(infoSesion.id_partida);

                //Pintamos los pueblos
                pintarCoordenadas();
            }
            else
            {
                MessageBox.Show("Ya no posees el pueblo");
                cbx_pueblos.DataSource = listaPueblos;
            }
        }

        private void btn_reclutamiento_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado actualmente
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            cbx_pueblos.DataSource = listaPueblos;

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

        private void btn_visionGeneral_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado actualmente
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            cbx_pueblos.DataSource = listaPueblos;

            try
            {
                // Comprobamos que el pueblo seleccionado sigue perteneciendo al jugador
                if (listaPueblos.FindAll(x => x.id_Pueblo == infoSesion.id_Pueblo).FirstOrDefault() == null)
                {
                    infoSesion.id_Pueblo = listaPueblos[0].id_Pueblo;

                }

                // Creamos un objeto del formulario de reclutamiento
                formVisionGeneral visionGeneral = new formVisionGeneral(infoSesion);

                // Lanzamos el formulario de reclutamiento
                visionGeneral.Show();

                // Cerramos este formulario
                this.Close();
            }
            catch
            {

            }
        }
        

        private void btn_movimientos_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado actualmente
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            cbx_pueblos.DataSource = listaPueblos;

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

        private void btn_Clasificacion_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado actualmente
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            cbx_pueblos.DataSource = listaPueblos;

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

        private void btn_mensajes_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado actualmente
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            cbx_pueblos.DataSource = listaPueblos;

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

        private void btn_mapa_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            // Introducimos los pueblos en el comboBox
            cbx_pueblos.DataSource = listaPueblos;

            if (listaPueblos.FindAll(x => x.id_Pueblo == infoSesion.id_Pueblo).FirstOrDefault() != null)
            {
                // Seleccionamos el pueblo anterior
                cbx_pueblos.SelectedValue = infoSesion.id_Pueblo;

            }
        }

        private void coordenadaDoubleClick(object sender, MouseEventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            if (listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault() != null)
            {
                PictureBox pbxAux = (PictureBox)sender;

                string coords = (string)pbxAux.Tag;

                infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

                // Creamos un objeto del formulario de inicio de sesion
                formMovimientos frm_Movimientos = new formMovimientos(infoSesion, coords);

                // Lanzamos el objeto de inicio de sesion   
                frm_Movimientos.Show();

                // Cerramos este formulario
                this.Close();
                

            }
            else
            {
                MessageBox.Show("Ya no posees el pueblo");
                cbx_pueblos.DataSource = listaPueblos;
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
                btn_volver_Click(null, null);
            }
        }

        // Comprobar si nos han quitado todos los pueblos
        private void comprobarPosesionPueblos()
        {
            // Obtenemos los pueblos del jugador
            listaPueblos = obtenerListaPueblos(infoSesion.nombreUsuario);

            if (listaPueblos.Count <= 0 || listaPueblos == null)
            {
                var userResponse = MessageBox.Show("Te han quitado todos los pueblos, perdiste la partida.");
                btn_volver_Click(null, null);
            }
        }
    }
}
