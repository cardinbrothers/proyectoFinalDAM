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
using System.Security.Cryptography;

namespace WindowsFormsDAMapp
{
    public partial class formInicioSesion : Form
    {
        sessionInfo infoSesion;
        RestClient restClient = new RestClient();
        webServiceInfo session = new webServiceInfo();


        public formInicioSesion(sessionInfo infoSesion)
        {
            this.infoSesion = infoSesion;

            InitializeComponent();
        }

        private void formInicioSesion_Load(object sender, EventArgs e)
        {
            restClient = new RestClient(session.CadenaConexion);

            lab_partida.Text = "Partida: " + Convert.ToInt32(infoSesion.id_partida);
        }

        private void Btn_iniciarSesion_Click(object sender, EventArgs e)
        {
            comprobarFinPartida();

            if (String.IsNullOrEmpty(tbx_nombreUsuario.Text) || String.IsNullOrEmpty(tbx_contraseña.Text))
            {
                MessageBox.Show("El usuario y la contraseña no pueden estar vacios");
            }
            else
            {
                // Creamos un objeto de entidad de jugador
                infoJugadorEntity Jugador = new infoJugadorEntity();

                string contraseñaEncriptada;
                bool accesoPermitido = false;

                // Encriptamos la contraseña
                using (MD5 hash = MD5.Create())
                {
                    byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(tbx_contraseña.Text));
                    StringBuilder sBuilder = new StringBuilder();

                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }

                    contraseñaEncriptada = sBuilder.ToString();
                }

                // Guardamos la informacion del usuario
                Jugador.nombreUsuario = tbx_nombreUsuario.Text;
                Jugador.contraseña = contraseñaEncriptada;
                Jugador.id_partida = infoSesion.id_partida;

                // Creamos un objeto para realizar la peticion el web service
                RestRequest peticion = new RestRequest("/api/Partida/iniciarSesion", Method.POST);

                // Añadimos la informacion del usuario a la peticion
                peticion.AddJsonBody(Jugador);

                // Obtenemos el resultado de la peticion
                var response = restClient.Execute(peticion);

                // Deserializamos el resultado de la peticion recibido para almacenarlo en un int
                int result = JsonConvert.DeserializeObject<int>(response.Content);

                string mensaje = "";

                // Segun el resultado recibido mandamos un mensaje
                switch (result)
                {
                    case 0:
                        accesoPermitido = true;
                        break;

                    case 1:
                        mensaje = "No existe el usuario para la partida indicada";
                        break;

                    case 2:
                        mensaje = "Contrasñe incorrecta";
                        break;

                    case 3:
                        mensaje = "Error desconocido";
                        break;
                }

                if (accesoPermitido)
                {
                    infoSesion.nombreUsuario = tbx_nombreUsuario.Text;

                    // Creamos un objeto del formulario de inicio de sesion
                    formVisionGeneral VisionGeneral = new formVisionGeneral(infoSesion);

                    // Lanzamos el objeto de inicio de sesion   
                    VisionGeneral.Show();

                    // Cerramos este formulario
                    this.Close();
                }
                else
                {
                    // Mostramos el mensaje
                    MessageBox.Show(mensaje);
                }


            }
        }

        private void Btn_crearCuenta_Click(object sender, EventArgs e)
        {
            comprobarFinPartida();

            if (String.IsNullOrEmpty(tbx_nombreUsuario.Text) || String.IsNullOrEmpty(tbx_contraseña.Text))
            {
                MessageBox.Show("El usuario y la contraseña no pueden estar vacios");
            }
            else
            {
                // Creamos un objeto de entidad de jugador
                infoJugadorEntity jugadorNuevo = new infoJugadorEntity();

                string contraseñaEncriptada;
                bool acccesoPermitido = false;

                // Encriptamos la contraseña
                using (MD5 hash = MD5.Create())
                {
                    byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(tbx_contraseña.Text));
                    StringBuilder sBuilder = new StringBuilder();

                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }

                    contraseñaEncriptada = sBuilder.ToString();
                }

                // Guardamos la informacion del usuario
                jugadorNuevo.nombreUsuario = tbx_nombreUsuario.Text;
                jugadorNuevo.contraseña = contraseñaEncriptada;
                jugadorNuevo.id_partida = infoSesion.id_partida;

                // Creamos un objeto para realizar la peticion el web service
                RestRequest peticion = new RestRequest("/api/Partida/crearJugador", Method.POST);

                // Añadimos la informacion del usuario nuevo a la peticion
                peticion.AddJsonBody(jugadorNuevo);

                // Obtenemos el resultado de la peticion
                var response = restClient.Execute(peticion);

                // Deserializamos el resultado de la peticion recibido para almacenarlo en un int
                int result = JsonConvert.DeserializeObject<int>(response.Content);

                string mensaje = "";

                // Segun el resultado recibido mandamos un mensaje
                switch (result)
                {
                    case 0:
                        acccesoPermitido = true;
                        break;

                    case 1:
                        mensaje = "Nombre de usuario ya existenteo";
                        break;

                    case 2:
                        mensaje = "Limite de jugadores para la partida ya alcanzado";
                        break;

                    case 3:
                        mensaje = "Partida expirada";
                        break;

                    case 4:
                        mensaje = "Error desconocido";
                        break;
                }

                if (acccesoPermitido)
                {
                    infoSesion.nombreUsuario = tbx_nombreUsuario.Text;

                    // Creamos un objeto del formulario de inicio de sesion
                    formVisionGeneral VisionGeneral = new formVisionGeneral(infoSesion);

                    // Lanzamos el objeto de inicio de sesion   
                    VisionGeneral.Show();

                    // Cerramos este formulario
                    this.Close();
                }
                else
                {
                    // Mostramos el mensaje
                    MessageBox.Show(mensaje);
                }
            }
        }

        private void Btn_volver_Click(object sender, EventArgs e)
        {
            frm_MenuPrincipal formularioPrincipal = new frm_MenuPrincipal();

            formularioPrincipal.Show();

            this.Close();
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
    }
}
