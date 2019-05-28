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
        List<puebloEntity> listaPueblosPropios;
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

            // Obtenemos los pueblos del jugador
            listaPueblosPropios = obtenerListaPueblos(infoSesion.nombreUsuario);

            // Introducimos los pueblos en el comboBox
            cbx_pueblos.ValueMember = "id_Pueblo";
            cbx_pueblos.DisplayMember = "coordenadas";
            cbx_pueblos.DataSource = listaPueblosPropios;

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
            string coordActual = listaPueblosPropios.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault().coordenadas;
            List<string> coordsPropias = new List<string>();

            foreach (var pueblo in listaPueblosPropios)
            {
                coordsPropias.Add(pueblo.coordenadas);
            }
            
            foreach (var pbx in this.Controls)
            {
                if (pbx is PictureBox)
                {
                    PictureBox pbxAux = (PictureBox)pbx;

                    if ((string)pbxAux.Tag == coordActual)
                    {
                        pbxAux.Image = Properties.Resources.punto_azul_oscuro;
                    }
                    else
                    {
                        if (coordsPropias.Contains((string)pbxAux.Tag))
                        {
                            pbxAux.Image = Properties.Resources.punto_azul_png;

                        }
                        else
                        {
                            if (listaCoordsPartida.Contains((string)pbxAux.Tag))
                            {
                                pbxAux.Image = Properties.Resources.punto_rojo;
                            }
                        }

                    }
                }
            }
                
            
        }
    }
}
