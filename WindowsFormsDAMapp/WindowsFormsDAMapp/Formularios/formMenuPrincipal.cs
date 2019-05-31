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

            // Ponemos valores por defecto en los comboBox
            cbx_Durarcion.SelectedIndex = 0;
            cbx_limiteJugadores.SelectedIndex = 0;
            cbx_limitePoblacion.SelectedIndex = 0;
            cbx_velocidad.SelectedIndex = 0;
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
                ListViewItem itemAux = new ListViewItem(partida.id_Partida.ToString());

                // Calculamos el tiempo restante de la partida y las plazas libres restantes
                TimeSpan tiempoRestante = (partida.fechaInicio.TimeOfDay + Convert.ToDateTime(partida.duracion).TimeOfDay) - DateTime.Now.TimeOfDay;
                int plazasLibres = partida.limiteJugadores - partida.jugadoresActivos;

                // Añadimos el resto de informacion al item de listview
                itemAux.SubItems.Add("x" + partida.velocidad.ToString());
                itemAux.SubItems.Add(Math.Round(tiempoRestante.TotalMinutes).ToString() + " minutos");
                itemAux.SubItems.Add(partida.limitePoblacion.ToString());
                itemAux.SubItems.Add(plazasLibres.ToString());

                // Añadimos el item al ListView
                objListView.Items.Add(itemAux);
            }
        }

        // Metodo asociado al evento click del boton recargar 
        private void Btn_recargar_Click(object sender, EventArgs e)
        {
            // Obtenemos la lista de partidas activas y las introducimos en el listview
            introducirPartidas(obtenerListaPartidas());
        }

        private void BtnCrearPartida_Click(object sender, EventArgs e)
        {
            // Creamos un objeto de entidad de partida
            infoPartidaEntity partidaNueva = new infoPartidaEntity();

            // Guardamos la informacion seleccionada por el usuario
            partidaNueva.velocidad = Convert.ToInt32(cbx_velocidad.SelectedItem.ToString());
            partidaNueva.duracion = new TimeSpan(0,Convert.ToInt32(cbx_Durarcion.SelectedItem.ToString()),0).ToString();
            partidaNueva.limiteJugadores = Convert.ToInt32(cbx_limiteJugadores.SelectedItem.ToString());
            partidaNueva.limitePoblacion = Convert.ToInt32(cbx_limitePoblacion.SelectedItem.ToString());


            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Partida/crearPartida", Method.POST);

            // Añadimos la informacion de la partida nueva a la peticion
            peticion.AddJsonBody(partidaNueva);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para tenerlo en una lista de objetos
            int result = JsonConvert.DeserializeObject<int>(response.Content);

            // Obtenemos la lista de partidas activas y las introducimos en el listview
            introducirPartidas(obtenerListaPartidas());

        }

        // Metodo asignado al evento de dobleClick del listview
        private void Lsv_PartidasActivas_DoubleClick(object sender, EventArgs e)
        {
            // Creamos un objeto de sesion
            sessionInfo infoSesion = new sessionInfo();

            // Introducimos el id de partida seleccinado
            infoSesion.id_partida = Convert.ToInt32(lsv_PartidasActivas.Items[lsv_PartidasActivas.SelectedIndices[0]].Text);

            // Creamos un objeto del formulario de inicio de sesion
            formInicioSesion inicioSesion = new formInicioSesion(infoSesion);

            // Lanzamos el objeto de inicio de sesion   
            inicioSesion.Show();

            // Cerramos este formulario
            this.Close();
        }

        // Metodo asociado al evento close del formulario
        private void Frm_MenuPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((Form)sender).FormClosed -= Frm_MenuPrincipal_FormClosed;

            // Comprobamos si hay otro formulario ejecutandose, en caso de que no cerramos la aplicacion
            if (Application.OpenForms.Count == 0)
            {
                Application.ExitThread();
            }
            else
            {
                Application.OpenForms[0].FormClosed += Frm_MenuPrincipal_FormClosed;
            }
        }
    }
}
