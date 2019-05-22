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
            restClient = new RestClient(session.CadenaConexion);
        }

        // Metodo que obtiene las partidas de la base de datos
        private List<infoPartidaEntity> obtenerListaPartidas()
        {
            RestRequest peticion = new RestRequest(session.CadenaConexion + "/api/Partida/partidasActivas", Method.GET);

            var lista = restClient.Execute(peticion);

            List<infoPartidaEntity> listaPartidas = JsonConvert.DeserializeObject<List<infoPartidaEntity>>(lista.Content);

            return listaPartidas;
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
