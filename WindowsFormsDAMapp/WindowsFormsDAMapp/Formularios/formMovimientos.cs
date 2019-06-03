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
    public partial class formMovimientos : Form
    {
        sessionInfo infoSesion;
        RestClient restClient = new RestClient();
        webServiceInfo session = new webServiceInfo();
        infoPartidaEntity paramsPartida;
        List<puebloEntity> listaPueblos;
        puebloEntity infoPueblo;
        tropasDefensivasEntity tropasDefReales;
        bool SalidaForm = false;

        public formMovimientos(sessionInfo infoSesion)
        {
            this.infoSesion = infoSesion;
            InitializeComponent();
        }

        public formMovimientos(sessionInfo infoSesion, string coordsObjetivo)
        {
            this.infoSesion = infoSesion;
            InitializeComponent();
            tbx_coordApoyo.Text = coordsObjetivo;
            tbx_CoordAtaque.Text = coordsObjetivo;

        }

        private void formMovimientos_Load(object sender, EventArgs e)
        {

            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Obtenemos los paramtros de configuracion de la partida
            paramsPartida = obtenerParametrosPartida(infoSesion.id_partida);

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            // Obtenemos la info del pueblo
            infoPueblo = obtenerInfoPueblo(infoSesion.id_Pueblo);

            // Obtenemos las tropas reales del pueblo
            tropasDefReales = obtenerTropas(infoSesion.id_Pueblo);

            // Introducimos los pueblos en el comboBox
            cbx_pueblos.ValueMember = "id_Pueblo";
            cbx_pueblos.DisplayMember = "coordenadas";
            cbx_pueblos.DataSource = listaPueblos;

            // Seleccionamos el pueblo deseado
            cbx_pueblos.SelectedValue = infoSesion.id_Pueblo;
        }

        // Metodo para obtener la info del pueblo
        private puebloEntity obtenerInfoPueblo(int idPueblo)
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Pueblo/obtenerPueblo", Method.GET);

            // Añadimos el nombre del usuario a la peticion
            peticion.AddParameter("id_Pueblo", idPueblo);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            puebloEntity result = JsonConvert.DeserializeObject<puebloEntity>(response.Content);

            return result;
        }

        // Metodo para obtener la info del pueblo
        private  tropasDefensivasEntity obtenerTropas (int idPueblo)
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Pueblo/obtenerDefReal", Method.GET);

            // Añadimos el nombre del usuario a la peticion
            peticion.AddParameter("id_Pueblo", idPueblo);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            tropasDefensivasEntity result = JsonConvert.DeserializeObject<tropasDefensivasEntity>(response.Content);

            return result;
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

        private void tbx_Tropas_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Comprobamos si el caracter introducido es diferente de un numero o una tecla de control
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btn_apoyar_Click(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();
            if (!SalidaForm)
            {
                if (listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault() != null)
                {
                    if (!String.IsNullOrEmpty(tbx_arquero.Text) || !String.IsNullOrEmpty(tbx_ballestero.Text))
                    {
                        // Creamos las variables para almacenar el numero de tropas
                        int arqueros, ballesteros;

                        // Creamos un objeto para realizar la peticion el web service
                        RestRequest peticion = new RestRequest("/api/Pueblo/obtenerId", Method.GET);

                        // Añadimos el id de la partida a la peticion
                        peticion.AddParameter("id_Partida", infoSesion.id_partida);

                        // Añadimos las coordenadas a la peticion
                        peticion.AddParameter("coordenada", tbx_coordApoyo.Text);

                        // Obtenemos el resultado de la peticion
                        var response = restClient.Execute(peticion);

                        // Deserializamos el resultado de la peticion recibido para almacenarlo
                        int idPuebloDestino = JsonConvert.DeserializeObject<int>(response.Content);

                        // Comprobamos que exista un pueblo con las coordenadas indicadas
                        if (idPuebloDestino != -1)
                        {
                            // Guardamos la cantidad de tropas introducidas por el usuario
                            if (!String.IsNullOrEmpty(tbx_arquero.Text))
                            {
                                arqueros = Convert.ToInt32(tbx_arquero.Text);
                            }
                            else
                            {
                                arqueros = 0;
                            }

                            if (!String.IsNullOrEmpty(tbx_ballestero.Text))
                            {
                                ballesteros = Convert.ToInt32(tbx_ballestero.Text);
                            }
                            else
                            {
                                ballesteros = 0;
                            }

                            if (arqueros <= tropasDefReales.arqueros && ballesteros <= tropasDefReales.ballesteros)
                            {

                                // Creamos un objeto para realizar la peticion el web service
                                RestRequest peticion2 = new RestRequest("/api/Pueblo/obtenerDistancia", Method.GET);

                                // Añadimos el id de la partida a la peticion
                                peticion2.AddParameter("id_Partida", infoSesion.id_partida);

                                // Añadimos las coordenadas a la peticion
                                peticion2.AddParameter("coordenada1", tbx_coordApoyo.Text);
                                peticion2.AddParameter("coordenada2", listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault().coordenadas);


                                // Obtenemos el resultado de la peticion
                                var response2 = restClient.Execute(peticion2);

                                // Deserializamos el resultado de la peticion recibido para almacenarlo
                                TimeSpan tiempoDistancia = JsonConvert.DeserializeObject<TimeSpan>(response2.Content);

                                // Creamos un nuevo movimiento
                                movimientossEntity nuevoMovimiento = new movimientossEntity();
                                nuevoMovimiento.puebloOrigen = (int)cbx_pueblos.SelectedValue;
                                nuevoMovimiento.puebloDestino = idPuebloDestino;
                                nuevoMovimiento.duracion = tiempoDistancia.ToString();
                                nuevoMovimiento.piqueros = 0;
                                nuevoMovimiento.caballeros = 0;
                                nuevoMovimiento.paladines = 0;
                                nuevoMovimiento.arqueros = arqueros;
                                nuevoMovimiento.ballesteros = ballesteros;
                                nuevoMovimiento.tipoMovimiento = "P";

                                // Creamos un objeto para realizar la peticion el web service
                                RestRequest peticion3 = new RestRequest("/api/Movimientos/realizarMovimiento", Method.POST);

                                // Añadimos el objeto
                                peticion3.AddJsonBody(nuevoMovimiento);

                                // Ejecutamos la peticion
                                var response3 = restClient.Execute(peticion3);

                                // Limpiamso los text boxes
                                tbx_arquero.Clear();
                                tbx_ballestero.Clear();

                                MessageBox.Show("Apoyo enviado correctamente");
                            }
                            else
                            {
                                MessageBox.Show("No tienes suficientes tropaspara enviar el apoyo");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cordenadas introducidas invalidas");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Has de introducir al menos una unidad");

                    }

                }
                else
                {
                    MessageBox.Show("Ya no posees el pueblo");
                    cbx_pueblos.DataSource = listaPueblos;
                }
            }      
        }

        private void btn_Atacar_Click(object sender, EventArgs e)
        {

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            if (!SalidaForm)
            {
                if (listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault() != null)
                {

                    if (!String.IsNullOrEmpty(tbx_piquero.Text) || !String.IsNullOrEmpty(tbx_caballero.Text) || !String.IsNullOrEmpty(tbx_paladin.Text))
                    {
                        // Creamos las variables para almacenar el numero de tropas
                        int piqueros, caballeros, paladines;

                        // Creamos un objeto para realizar la peticion el web service
                        RestRequest peticion = new RestRequest("/api/Pueblo/obtenerId", Method.GET);

                        // Añadimos el id de la partida a la peticion
                        peticion.AddParameter("id_Partida", infoSesion.id_partida);

                        // Añadimos las coordenadas a la peticion
                        peticion.AddParameter("coordenada", tbx_CoordAtaque.Text);

                        // Obtenemos el resultado de la peticion
                        var response = restClient.Execute(peticion);

                        // Deserializamos el resultado de la peticion recibido para almacenarlo
                        int idPuebloDestino = JsonConvert.DeserializeObject<int>(response.Content);

                        // Comprobamos que exista un pueblo con las coordenadas indicadas
                        if (idPuebloDestino != -1)
                        {
                            // Guardamos la cantidad de tropas introducidas por el usuario
                            if (!String.IsNullOrEmpty(tbx_piquero.Text))
                            {
                                piqueros = Convert.ToInt32(tbx_piquero.Text);
                            }
                            else
                            {
                                piqueros = 0;
                            }

                            if (!String.IsNullOrEmpty(tbx_caballero.Text))
                            {
                                caballeros = Convert.ToInt32(tbx_caballero.Text);
                            }
                            else
                            {
                                caballeros = 0;
                            }

                            if (!String.IsNullOrEmpty(tbx_paladin.Text))
                            {
                                paladines = Convert.ToInt32(tbx_paladin.Text);
                            }
                            else
                            {
                                paladines = 0;
                            }

                            if (piqueros <= infoPueblo.piqueros && caballeros <= infoPueblo.caballeros && paladines <= infoPueblo.paladines)
                            {

                                // Creamos un objeto para realizar la peticion el web service
                                RestRequest peticion2 = new RestRequest("/api/Pueblo/obtenerDistancia", Method.GET);

                                // Añadimos el id de la partida a la peticion
                                peticion2.AddParameter("id_Partida", infoSesion.id_partida);

                                // Añadimos las coordenadas a la peticion
                                peticion2.AddParameter("coordenada1", tbx_CoordAtaque.Text);
                                peticion2.AddParameter("coordenada2", listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault().coordenadas);


                                // Obtenemos el resultado de la peticion
                                var response2 = restClient.Execute(peticion2);

                                // Deserializamos el resultado de la peticion recibido para almacenarlo
                                TimeSpan tiempoDistancia = JsonConvert.DeserializeObject<TimeSpan>(response2.Content);

                                // Creamos un nuevo movimiento
                                movimientossEntity nuevoMovimiento = new movimientossEntity();
                                nuevoMovimiento.puebloOrigen = (int)cbx_pueblos.SelectedValue;
                                nuevoMovimiento.puebloDestino = idPuebloDestino;
                                nuevoMovimiento.duracion = tiempoDistancia.ToString();
                                nuevoMovimiento.piqueros = piqueros;
                                nuevoMovimiento.caballeros = caballeros;
                                nuevoMovimiento.paladines = paladines;
                                nuevoMovimiento.arqueros = 0;
                                nuevoMovimiento.ballesteros = 0;
                                nuevoMovimiento.tipoMovimiento = "A";

                                // Creamos un objeto para realizar la peticion el web service
                                RestRequest peticion3 = new RestRequest("/api/Movimientos/realizarMovimiento", Method.POST);

                                // Añadimos el objeto
                                peticion3.AddJsonBody(nuevoMovimiento);

                                // Ejecutamos la peticion
                                restClient.Execute(peticion3);

                                // Limpiamso los text boxes
                                tbx_arquero.Clear();
                                tbx_ballestero.Clear();

                                MessageBox.Show("Ataque enviado correctamente");
                            }
                            else
                            {
                                MessageBox.Show("No dispones de tropas suficientes para realizar el ataque");

                            }

                        }
                        else
                        {
                            MessageBox.Show("Cordenadas introducidas invalidas");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Has de introducir al menos una unidad");

                    }

                }
                else
                {
                    MessageBox.Show("Ya no posees el pueblo");
                    cbx_pueblos.DataSource = listaPueblos;
                }
            }

            
        }

        private void btn_volver_Click(object sender, EventArgs e)
        {
            frm_MenuPrincipal formularioPrincipal = new frm_MenuPrincipal();

            formularioPrincipal.Show();

            this.Close();
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
                if (!SalidaForm)
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
                if (!SalidaForm)
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
                if (!SalidaForm)
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

            if (listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault() != null)
            {
                // Obtenemos el pueblo seleccionado actualmente
                infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

                cbx_pueblos.DataSource = listaPueblos;

                // Seleccionamos el pueblo anterior
                cbx_pueblos.SelectedValue = infoSesion.id_Pueblo;

            }
            else
            {
                MessageBox.Show("Ya no posees el pueblo");
                cbx_pueblos.DataSource = listaPueblos;
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
                if (!SalidaForm)
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
            }
            catch
            {

            }
        }

        private void Cbx_pueblos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            if (listaPueblos.FindAll(x => x.id_Pueblo == (int)cbx_pueblos.SelectedValue).FirstOrDefault() != null)
            {
                // Almacenamos el id del pueblo seleccionado
                int id_Pueblo = (int)cbx_pueblos.SelectedValue;

                infoPueblo = obtenerInfoPueblo(id_Pueblo);
                tropasDefReales = obtenerTropas(id_Pueblo);
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
                if (!SalidaForm)
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
                btn_volver_Click(null, null);
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

        private void formMovimientos_FormClosing(object sender, FormClosingEventArgs e)
        {
            SalidaForm = true;
        }
    }
}
