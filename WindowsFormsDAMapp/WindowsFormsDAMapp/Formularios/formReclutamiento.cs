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
    public partial class formReclutamiento : Form
    {
        sessionInfo infoSesion;
        RestClient restClient = new RestClient();
        webServiceInfo session = new webServiceInfo();
        infoPartidaEntity paramsPartida;
        List<puebloEntity> listaPueblos;

        public formReclutamiento(sessionInfo infoSesion)
        {
            this.infoSesion = infoSesion;
            InitializeComponent();
        }

        private void formReclutamiento_Load(object sender, EventArgs e)
        {
            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);

            // Obtenemos los paramtros de configuracion de la partida
            paramsPartida = obtenerParametrosPartida(infoSesion.id_partida);

            // Obtenemos los pueblos del jugador
            listaPueblos = obtenerListaPueblos(infoSesion.nombreUsuario);

            // Introducimos los pueblos en el comboBox
            cbx_pueblos.ValueMember = "id_Pueblo";
            cbx_pueblos.DisplayMember = "coordenadas";
            cbx_pueblos.DataSource = listaPueblos;

            // Seleccionamos el pueblo deseado
            cbx_pueblos.SelectedValue = infoSesion.id_Pueblo;
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

        private void cbx_pueblos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbx_pueblos.Items.Count > 0 && cbx_pueblos.DataSource != null)
            {
                // Almacenamos el id del pueblo seleccionado
                int id_Pueblo = (int)cbx_pueblos.SelectedValue;

                // Obtenemos la informacion del pueblo seleccionado
                var pueblo = listaPueblos.FindAll(x => x.id_Pueblo == id_Pueblo).FirstOrDefault();

                // Introducimos la poblacion en el label correspondiente
                lab_PoblacionPueblo.Text = String.Format("Poblacion: {0}/{1}", pueblo.poblacionRestante, paramsPartida.limitePoblacion);
              
            }

        }

        // Metodo para solo permitir escribir en un texbox de tropa a la vez
        private void textBoxTextChanged(object sender, EventArgs e)
        {
            TextBox tbxAux = (TextBox)sender;

            // Comprobamos si el textbox contiene texto
            if (tbxAux.Text != "")
            {
                // Deshabilitamos todos los textboxes
                tbx_arquero.ReadOnly = true;
                tbx_piquero.ReadOnly = true;
                tbx_ballestero.ReadOnly = true;
                tbx_caballero.ReadOnly = true;
                tbx_paladin.ReadOnly = true;

                // Habilitamos el que se esta cambiando
                tbxAux.ReadOnly = false;
               
            }
            else
            {
                // Habilitamos todos los textboxes
                tbx_arquero.ReadOnly = false;
                tbx_piquero.ReadOnly = false;
                tbx_ballestero.ReadOnly = false;
                tbx_caballero.ReadOnly = false;
                tbx_paladin.ReadOnly = false;
            }
        }

        // Metodo para controlar los caracteres introducidos en los textboxes de tropas
        private void tbxTropa_KeyPressed(object sender, KeyPressEventArgs e)
        {
            // Comprobamos si el caracter introducido es diferente de un numero o una tecla de control
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btn_reclutar_Click(object sender, EventArgs e)
        {
            int id_Pueblo = 0, id_Tropa = 0, cantidad = 0;

            id_Pueblo = (int)cbx_pueblos.SelectedValue;
            
            foreach (var tbx in this.Controls)
            {
                if (tbx is TextBox)
                {
                    TextBox tbxAux = (TextBox)tbx;

                    if (!String.IsNullOrEmpty(tbxAux.Text))
                    {
                        id_Tropa = Convert.ToInt32(tbxAux.Tag);
                        cantidad = Convert.ToInt32(tbxAux.Text);

                    }
                }
            }

            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Reclutamiento/reclutarTropas", Method.POST);

            // Añadimos el nombre del usuario a la peticion
            peticion.AddQueryParameter("idPueblo", id_Pueblo.ToString());

            // Añadimos el nombre del usuario a la peticion
            peticion.AddQueryParameter("idTropa", id_Tropa.ToString());

            // Añadimos el nombre del usuario a la peticion
            peticion.AddQueryParameter("cantidad", cantidad.ToString());

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            ordenReclutamientoEntity result = JsonConvert.DeserializeObject<ordenReclutamientoEntity>(response.Content);

            int a = 0;
        }

    }
}
