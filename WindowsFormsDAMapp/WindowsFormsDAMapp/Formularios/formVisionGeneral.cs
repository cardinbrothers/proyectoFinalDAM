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
    public partial class formVisionGeneral : Form
    {
        sessionInfo infoSesion;
        RestClient restClient = new RestClient();
        webServiceInfo session = new webServiceInfo();
        infoPartidaEntity paramsPartida;
        List<puebloEntity> listaPueblos;

        public formVisionGeneral(sessionInfo infoSesion)
        {
            this.infoSesion = infoSesion;
            InitializeComponent();
        }

        private void FormVisionGeneral_Load(object sender, EventArgs e)
        {
            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);

            // Obtenemos los paramtros de configuracion de la partida
            paramsPartida = obtenerParametrosPartida(infoSesion.id_partida);

            // Obtenemos los pueblos del jugador
            listaPueblos = obtenerListaPueblos(infoSesion.nombreUsuario);

            // obtenemos los movimientos del pueblo

            // Introducimos los pueblos en el comboBox
            cbx_pueblos.ValueMember = "id_Pueblo";
            cbx_pueblos.DisplayMember = "coordenadas";
            cbx_pueblos.DataSource = listaPueblos;

        }

        // Metodo para obtener los parametros de la partida
        private infoPartidaEntity obtenerParametrosPartida(int id_Partida)
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Partida/parametrosPartida", Method.GET);

            // Añadimos el id de la partida a la peticion
            peticion.AddParameter("id_Partida", id_Partida);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            infoPartidaEntity result = JsonConvert.DeserializeObject<infoPartidaEntity>(response.Content);

            return result;
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

        // Introducimmos los valores del pueblo de poblacion, tropas y los movimientos
        private void cbx_pueblos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbx_pueblos.Items.Count > 0 && cbx_pueblos.DataSource != null)
            {
                // Almacenamos el id del pueblo seleccionado
                int id_Pueblo = (int)cbx_pueblos.SelectedValue;

                // Obtenemos la informacion del pueblo seleccionado
                var pueblo = listaPueblos.FindAll(x => x.id_Pueblo == id_Pueblo).FirstOrDefault();

                // Introducimos la poblacion en el label correspondiente
                lab_poblacion.Text = String.Format("Poblacion: {0}/{1}", pueblo.poblacionRestante, paramsPartida.limitePoblacion);

                // Cambiamos el valor de las tropas
                for (int i = 0; i < 5; i++)
                {
                    switch (i)
                    {
                        case 0:
                            lsv_tropas.Items[i].SubItems[1].Text = pueblo.arqueros.ToString();
                            break;
                        case 1:
                            lsv_tropas.Items[i].SubItems[1].Text = pueblo.ballesteros.ToString();
                            break;
                        case 2:
                            lsv_tropas.Items[i].SubItems[1].Text = pueblo.piqueros.ToString();
                            break;
                        case 3:
                            lsv_tropas.Items[i].SubItems[1].Text = pueblo.caballeros.ToString();
                            break;
                        case 4:
                            lsv_tropas.Items[i].SubItems[1].Text = pueblo.paladines.ToString();
                            break;
                    }
                }

                // Actualizamos los movimientos
            }

        }

        // Boton para cerrar sesion
        private void btn_volver_Click(object sender, EventArgs e)
        {
            frm_MenuPrincipal formularioPrincipal = new frm_MenuPrincipal();

            formularioPrincipal.Show();

            this.Close();
        }

        // Metodo asociado al evento recargar que lanza de nuevo la vision general
        private void btn_visionGeneral_Click(object sender, EventArgs e)
        {
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;
            cbx_pueblos.DataSource = null;
            cbx_pueblos.Items.Clear();

            listaPueblos = obtenerListaPueblos(infoSesion.nombreUsuario);
            cbx_pueblos.ValueMember = "id_Pueblo";
            cbx_pueblos.DisplayMember = "coordenadas";
            cbx_pueblos.DataSource = listaPueblos;
            try
            {
                cbx_pueblos.SelectedValue = infoSesion.id_Pueblo;
            }
            catch
            {

            }
        }

        // Metodo asociado al boton reclutamiento para lanzar el formulario reclutamiento
        private void btn_reclutamiento_Click(object sender, EventArgs e)
        {
            // Añadimos el id del pueblo actual
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Creamos un objeto del formulario de reclutamiento
            formReclutamiento reclutamiento = new formReclutamiento(infoSesion);

            // Lanzamos el formulario de reclutamiento
            reclutamiento.Show();

            // Cerramos este formulario
            this.Close();
        }
    }
}
