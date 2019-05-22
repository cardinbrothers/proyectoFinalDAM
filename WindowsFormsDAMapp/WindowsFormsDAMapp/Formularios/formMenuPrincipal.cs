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
    public partial class frm_MenuPrincipal : Form
    {
        webServiceInfo session = new webServiceInfo();
        RestClient restClient = new RestClient();

        public frm_MenuPrincipal()
        {
            InitializeComponent();
        }

        // Metodo que maneja el evento de carga del funcionario
        private void frm_MenuPrincipal_Load(object sender, EventArgs e)
        {
            // Creamos el objeto para acceder al web service
            restClient = new RestClient(session.CadenaConexion);

            // Obtenemos la lista de partidas activas y las introducimos en el listview
            introducirPartidas(obtenerListaPartidas());
        }

        // Metodo que obtiene las partidas de la base de datos
        private List<infoPartidaEntity> obtenerListaPartidas()
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Partida/partidasActivas", Method.GET);

            // Obtenemos el resultado de la peticion
            var lista = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para tenerlo en una lista de objetos
            List<infoPartidaEntity> listaPartidas = JsonConvert.DeserializeObject<List<infoPartidaEntity>>(lista.Content);

            // Devolvemos la lista de partidas
            return listaPartidas;
        }

        // Metodo que introducie en el listview de partidas las partidas recibidas
        private void introducirPartidas(List<infoPartidaEntity> listaPartidas)
        {
            // Obtenemos el listview de partidas
            ListView objListView = this.lsv_PartidasActivas;

            // Borramos los registros antiguos
            objListView.Items.Clear();

            // Por cada partida recibida realizamos lo siguiente
            foreach (var partida in listaPartidas)
            {
                // Creamos unListViewItem auxiliar y almacenamos el id de partida
                ListViewItem itemAux = new ListViewItem("Partida " + partida.id_Partida.ToString());

                // Calculamos el tiempo restante de la partida y las plazas libres restantes
                TimeSpan tiempoRestante = (partida.fechaInicio.TimeOfDay + partida.duracion) - DateTime.Now.TimeOfDay;
                int plazasLibres = partida.limiteJugadores - partida.jugadoresActivos;

                // Añadimos el resto de informacion al item de listview
                itemAux.SubItems.Add("x" + partida.velocidad.ToString());
                itemAux.SubItems.Add(Math.Round(tiempoRestante.TotalMinutes).ToString() + " minutos");
                itemAux.SubItems.Add(plazasLibres.ToString());

                // Añadimos el item al ListView
                objListView.Items.Add(itemAux);
            }
        }

        private void Btn_recargar_Click(object sender, EventArgs e)
        {
            // Obtenemos la lista de partidas activas y las introducimos en el listview
            introducirPartidas(obtenerListaPartidas());
        }

        //public async Task<string> InsertFichaje(RegisterRequest registerRequest)
        //{

        //    RegisterResult registerResult;
        //    var response = "3";


        //    // Controlador/metodo en el new restrequest
        //    var request = new RestRequest("AccessRecords/one", Method.POST);
        //    request.AddJsonBody(new ApiAccessRecord
        //    {
        //        accessPointId = registerRequest.TagId,
        //        Type = (int)registerRequest.Type,
        //        Id = registerRequest.IdUser

        //    });

        //    try
        //    {

        //        response = Execute<ApiAccessRecord>(request);


        //    }
        //    catch (Exception e)
        //    {

        //        Console.Write(e.Message);
        //        registerResult = new RegisterResult { Result = RegisterResults.Exception };
        //    }



        //    return response;
        //}
    }
}
