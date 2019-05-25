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
        List<mensajesEntity> listaMensajes;

        public formEnviarMensaje(sessionInfo info)
        {
            infoSesion = info;
            InitializeComponent();
        }

        private void FormEnviarMensaje_Load(object sender, EventArgs e)
        {
            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);

            // Obtenemos la lista de mensajes
            listaMensajes = obtenerMensajes();

            // Introducimos los mensajes en el listView

        }

        private List<mensajesEntity> obtenerMensajes()
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Mensaje/obtenerMensajesP", Method.GET);

            // Añadimos el id de la partida a la peticion
            peticion.AddParameter("id_Partida", infoSesion.nombreUsuario);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            List<mensajesEntity> result = JsonConvert.DeserializeObject<List<mensajesEntity>>(response.Content);

            return result;
        }

        private void mostrarMensajes(List<mensajesEntity> mensajes)
        {
             
        }
    }
    
}
