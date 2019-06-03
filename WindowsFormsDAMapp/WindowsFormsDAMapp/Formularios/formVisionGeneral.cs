﻿using System;
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
using System.Threading;

namespace WindowsFormsDAMapp
{
    public partial class formVisionGeneral : Form
    {
        sessionInfo infoSesion;
        RestClient restClient = new RestClient();
        webServiceInfo session = new webServiceInfo();
        infoPartidaEntity paramsPartida;
        List<puebloEntity> listaPueblos;
        List<movimientossEntity> listaMovimientos;
        tropasDefensivasEntity cantDeffPropias;

        public formVisionGeneral(sessionInfo infoSesion)
        {
            this.infoSesion = infoSesion;
            InitializeComponent();
        }

        private void FormVisionGeneral_Load(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);

            // Obtenemos los paramtros de configuracion de la partida
            paramsPartida = obtenerParametrosPartida(infoSesion.id_partida);

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            if (infoSesion.id_Pueblo == 0)
            {
                infoSesion.id_Pueblo = listaPueblos.First().id_Pueblo;
            }

            // obtenemos los movimientos del pueblo
            listaMovimientos = obtenerMovimientos(infoSesion.id_Pueblo);

            // Introducir los movimientos en listView
            mostrarMovimientos(listaMovimientos);

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

        // Metodo para obtener la info del pueblo
        private tropasDefensivasEntity obtenerTropasPropias(int idPueblo)
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

        // Introducimmos los valores del pueblo de poblacion, tropas y los movimientos
        private void cbx_pueblos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            // Comprobamos si se posee al menos un pueblo y los almacenamos
            comprobarPosesionPueblos();

            if (cbx_pueblos.Items.Count > 0 && cbx_pueblos.DataSource != null)
            {
                // Almacenamos el id del pueblo seleccionado
                int id_Pueblo = (int)cbx_pueblos.SelectedValue;

                // Obtenemos la informacion del pueblo seleccionado
                var pueblo = listaPueblos.FindAll(x => x.id_Pueblo == id_Pueblo).FirstOrDefault();

                // Introducimos la poblacion en el label correspondiente
                lab_poblacion.Text = String.Format("Poblacion: {0}/{1}", pueblo.poblacionRestante, paramsPartida.limitePoblacion);

                // Obtenemos las tropas deff propias
                cantDeffPropias = obtenerTropasPropias(id_Pueblo);

                // Cambiamos el valor de las tropas
                for (int i = 0; i < 5; i++)
                {
                    switch (i)
                    {
                        case 0:
                            lsv_tropas.Items[i].SubItems[1].Text = cantDeffPropias.arqueros.ToString();
                            lsv_tropas.Items[i].SubItems[2].Text = (pueblo.arqueros - cantDeffPropias.arqueros).ToString();
                            break;
                        case 1:
                            lsv_tropas.Items[i].SubItems[1].Text = cantDeffPropias.ballesteros.ToString();
                            lsv_tropas.Items[i].SubItems[2].Text = (pueblo.ballesteros - cantDeffPropias.ballesteros).ToString();

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
                listaMovimientos = obtenerMovimientos(id_Pueblo);
                mostrarMovimientos(listaMovimientos);
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

        // Metodo asociado al boton reclutamiento para lanzar el formulario reclutamiento
        private void btn_reclutamiento_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado actualmente
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            if (cbx_pueblos.SelectedValue != null)
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

        private void Btn_Clasificacion_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado actualmente
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            if (cbx_pueblos.SelectedValue != null)
            {
                // Añadimos el id del pueblo actual
                infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

                // Creamos un objeto del formulario de reclutamiento
                formClasificacion clasificacion = new formClasificacion(infoSesion);

                // Lanzamos el formulario de reclutamiento
                clasificacion.Show();

                // Cerramos este formulario
                this.Close();
            }
        }

        private void Btn_mensajes_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado actualmente
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            if (cbx_pueblos.SelectedValue != null)
            {
                // Añadimos el id del pueblo actual
                infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

                // Creamos un objeto del formulario de reclutamiento
                formBandejaEntrada bandejaEntrada = new formBandejaEntrada(infoSesion);

                // Lanzamos el formulario de reclutamiento
                bandejaEntrada.Show();

                // Cerramos este formulario
                this.Close();
            }
        }

        private void btn_movimientos_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado actualmente
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            if (cbx_pueblos.SelectedValue != null)
            {
                infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

                // Creamos un objeto del formulario de inicio de sesion
                formMovimientos frm_Movimientos = new formMovimientos(infoSesion);

                // Lanzamos el objeto de inicio de sesion   
                frm_Movimientos.Show();

                // Cerramos este formulario
                this.Close();
            }
        }

        private List<movimientossEntity> obtenerMovimientos(int id_Pueblo)
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Movimientos/obtenerMovimientos", Method.GET);

            // Añadimos el id de la partida a la peticion
            peticion.AddParameter("id_Pueblo", id_Pueblo);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            List<movimientossEntity> result = JsonConvert.DeserializeObject<List<movimientossEntity>>(response.Content);

            return result;
        }

        private void mostrarMovimientos(List<movimientossEntity> listaMovimientos)
        {
            lsv_entrantes.Items.Clear();
            lsv_Salientes.Items.Clear();

            foreach (var movimiento in listaMovimientos)
            {
                if (movimiento.puebloOrigen == infoSesion.id_Pueblo)
                {
                    ListViewItem itemAux = new ListViewItem(movimiento.puebloDestino.ToString());
                    itemAux.SubItems.Add((movimiento.horaLlegada - DateTime.Now).ToString(@"hh\:mm\:ss"));
                    itemAux.SubItems.Add(movimiento.horaLlegada.ToString());
                    if (movimiento.tipoMovimiento == "P")
                    {
                        itemAux.SubItems.Add("Apoyo");

                    }
                    else
                    {
                        itemAux.SubItems.Add("Ataque");

                    }

                    lsv_Salientes.Items.Add(itemAux);
                }
                else
                {
                    ListViewItem itemAux = new ListViewItem(movimiento.puebloOrigen.ToString());
                    itemAux.SubItems.Add((movimiento.horaLlegada - DateTime.Now).ToString(@"hh\:mm\:ss"));
                    itemAux.SubItems.Add(movimiento.horaLlegada.ToString());
                    if (movimiento.tipoMovimiento == "P")
                    {
                        itemAux.SubItems.Add("Apoyo");

                    }
                    else
                    {
                        itemAux.SubItems.Add("Ataque");

                    }

                    lsv_entrantes.Items.Add(itemAux);
                }
            }
        }

        private void btn_mapa_Click(object sender, EventArgs e)
        {
            // Obtenemos el pueblo seleccionado actualmente
            infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

            // Comprobar si se ha acabado la partida
            comprobarFinPartida();

            if (cbx_pueblos.SelectedValue != null)
            {
                infoSesion.id_Pueblo = (int)cbx_pueblos.SelectedValue;

                // Creamos un objeto del formulario de inicio de sesion
                frmMapa frm_mapa = new frmMapa(infoSesion);

                // Lanzamos el objeto de inicio de sesion   
                frm_mapa.Show();

                // Cerramos este formulario
                this.Close();
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

            if (listaPueblos.Count <= 0)
            {
                var userResponse = MessageBox.Show("Te han quitado todos los pueblos, perdiste la partida.");
                btn_volver_Click(null, null);
            }
        }

    }
}
