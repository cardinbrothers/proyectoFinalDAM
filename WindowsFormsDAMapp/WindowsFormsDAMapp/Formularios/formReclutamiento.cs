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
        List<ordenReclutamientoEntity> listaOrdenes;
        bool SalidaForm = false;

        public formReclutamiento(sessionInfo infoSesion)
        {
            this.infoSesion = infoSesion;
            InitializeComponent();
        }

        private void formReclutamiento_Load(object sender, EventArgs e)
        {

            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            // Obtenemos los paramtros de configuracion de la partida
            paramsPartida = obtenerParametrosPartida(infoSesion.id_partida);

            // Introducimos el tiempo de las tropas segun la velocidad
            tiempoTropas();

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

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            if (listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault() != null)
            {
                // Almacenamos el id del pueblo seleccionado
                int id_Pueblo = (int)cbx_pueblos.SelectedValue;

                // Obtenemos la informacion del pueblo seleccionado
                var pueblo = listaPueblos.FindAll(x => x.id_Pueblo == id_Pueblo).FirstOrDefault();

                // Introducimos la poblacion en el label correspondiente
                lab_PoblacionPueblo.Text = String.Format("Poblacion: {0}/{1}", pueblo.poblacionRestante, paramsPartida.limitePoblacion);

                // Obtenemos las ordenes de reclutamiento   
                lsv_cola.Items.Clear();
                obtenerOrdenes(id_Pueblo);

            }
            else
            {
                if (!SalidaForm)
                {
                    MessageBox.Show("Ya no posees el pueblo");
                    cbx_pueblos.DataSource = listaPueblos;
                }
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
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            if (listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault() != null)
            {

                int id_Pueblo = 0, id_Tropa = -1, cantidad = 0;

                id_Pueblo = (int)cbx_pueblos.SelectedValue;

                // Obtenemos el id tropa y cantidad de los textbox
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

                // Comprobamos si hay algun dato en algun textboxç
                if (id_Tropa != -1 && cantidad > 0)
                {
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

                    switch (result.error)
                    {
                        case 0:
                            Btn_reclutamiento_Click(null, null);
                            break;
                        case 1:
                            MessageBox.Show("No tienes suficiente poblacion");
                            break;
                        case 2:
                            MessageBox.Show("Algo salio mal");
                            break;

                    }
                }
                else
                {
                    MessageBox.Show("No se han elegido unidades");
                }

            }
            else
            {
                MessageBox.Show("Ya no posees el pueblo");
                cbx_pueblos.DataSource = listaPueblos;
            }


        }

        private void introducirReclutamiento(ordenReclutamientoEntity orden)
        {
            string nombreTropa = "";

            // Obtenemos el nombre de la tropa
            switch (orden.id_Tropa)
            {
                case 1:
                    nombreTropa = "Arqueros";
                    break;
                case 2:
                    nombreTropa = "Ballesteros";
                    break;
                case 4:
                    nombreTropa = "Piqueros";
                    break;
                case 7:
                    nombreTropa = "Caballeros";
                    break;
                case 10:
                    nombreTropa = "Paladines";
                    break;
            }

            ListViewItem itemAux = new ListViewItem(nombreTropa);

            // Añadimos la cantidad y timepo restante al listview
            itemAux.SubItems.Add(orden.cantidad.ToString());
            itemAux.SubItems.Add((orden.horaFin - DateTime.Now).ToString(@"hh\:mm\:ss"));

            lsv_cola.Items.Add(itemAux);

        }

        private void obtenerOrdenes(int id_Pueblo)
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Reclutamiento/obtenerOrdenes", Method.GET);

            // Añadimos el id del pueblo a la peticion
            peticion.AddParameter("idPueblo", id_Pueblo.ToString());

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            List<ordenReclutamientoEntity> result = JsonConvert.DeserializeObject<List<ordenReclutamientoEntity>>(response.Content);

            listaOrdenes = result;

            // Introducimos las ordenes en el listview
            foreach (var orden in listaOrdenes)
            {
                introducirReclutamiento(orden);
            }
        }

        // Metodo para introducir el tiempo que tarda cada tropa segun la velocidad de la partida
        private void tiempoTropas()
        {
            // Creamos los objetos TimeSpan con los tiempos
            TimeSpan arqueros = new TimeSpan(0, 0, 60 / paramsPartida.velocidad);
            TimeSpan ballesteros = new TimeSpan(0, 0, 300 / paramsPartida.velocidad); ;
            TimeSpan piqueros = new TimeSpan(0, 0, 60 / paramsPartida.velocidad);
            TimeSpan caballeros = new TimeSpan(0, 0, 420 / paramsPartida.velocidad);
            TimeSpan paladines = new TimeSpan(0, 0, 1200 / paramsPartida.velocidad);

            // Introducimos los tiempos en los labels
            lab_tiempo1.Text = arqueros.ToString();
            lab_tiempo2.Text = ballesteros.ToString();
            lab_tiempo3.Text = piqueros.ToString();
            lab_tiempo4.Text = caballeros.ToString();
            lab_tiempo5.Text = paladines.ToString();
        }

        private void Btn_visionGeneral_Click(object sender, EventArgs e)
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
        
        // Metodo para recargar la vista
        private void Btn_reclutamiento_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado actualmente
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            cbx_pueblos.DataSource = listaPueblos;

            tbx_arquero.ReadOnly = false;
            tbx_piquero.ReadOnly = false;
            tbx_ballestero.ReadOnly = false;
            tbx_caballero.ReadOnly = false;
            tbx_paladin.ReadOnly = false;
            tbx_arquero.Clear();
            tbx_piquero.Clear();
            tbx_ballestero.Clear();
            tbx_caballero.Clear();
            tbx_paladin.Clear();

            try
            {
                // Comprobamos que el pueblo seleccionado sigue perteneciendo al jugador
                if (listaPueblos.FindAll(x => x.id_Pueblo == infoSesion.id_Pueblo).FirstOrDefault() != null)
                {
                    // Seleccionamos el pueblo anterior
                    cbx_pueblos.SelectedValue = infoSesion.id_Pueblo;

                }
            }
            catch
            {

            }
        }

        private void Btn_Clasificacion_Click(object sender, EventArgs e)
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

        private void Btn_cancelar_Click(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            if (listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault() != null)
            {

                if (lsv_cola.SelectedItems.Count > 0)
                {
                    int id_Orden = listaOrdenes[lsv_cola.SelectedIndices[0]].id_Orden;

                    // Creamos un objeto para realizar la peticion el web service
                    RestRequest peticion = new RestRequest("/api/Reclutamiento/cancelar", Method.POST);

                    // Añadimos el nombre del usuario a la peticion
                    peticion.AddQueryParameter("idOrden", id_Orden.ToString());

                    // Obtenemos el resultado de la peticion
                    var response = restClient.Execute(peticion);

                    // Deserializamos el resultado de la peticion recibido para almacenarlo
                    int result = JsonConvert.DeserializeObject<int>(response.Content);

                    // Actualizamos
                    Btn_reclutamiento_Click(null, null);

                }

            }
            else
            {
                MessageBox.Show("Ya no posees el pueblo");
                cbx_pueblos.DataSource = listaPueblos;
            }

            
        }

        // Metodo para volver al menu principal
        private void Btn_volver_Click(object sender, EventArgs e)
        {
            frm_MenuPrincipal formularioPrincipal = new frm_MenuPrincipal();

            formularioPrincipal.Show();

            this.Close();
        }

        private void Btn_mensajes_Click(object sender, EventArgs e)
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

        private void btn_mapa_Click(object sender, EventArgs e)
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
                    btn_volver_Click(null, null);
                }
            }
        }

        private void formReclutamiento_FormClosing(object sender, FormClosingEventArgs e)
        {
            SalidaForm = true;
        }
    }
}
