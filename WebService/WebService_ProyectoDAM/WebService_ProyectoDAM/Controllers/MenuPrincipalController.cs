using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebService_ProyectoDAM.ApiEntities;
using WebService_ProyectoDAM.Servicios;
using Newtonsoft.Json;
using System.Text;

namespace WebService_ProyectoDAM.Controllers
{
    [RoutePrefix("api/Partida")]
    public class MenuPrincipalController : ApiController
    {

        servicioMenuPrincipal servicio = new servicioMenuPrincipal();

        [Route("ping")]
        [HttpGet]
        public IHttpActionResult Ping()
        {
            string a = System.Web.HttpUtility.UrlEncode("alamás", Encoding.UTF7);
            return Ok("pong!");
        }

        [Route("prueba")]
        [HttpPost]
        public string GetByFilter([FromBody] infoPartidaEntity record)
        {
            var json = JsonConvert.SerializeObject(record);

            return json;
        }

        [Route("crearPartida")]
        [HttpPost]
        public int crearPartida([FromBody] infoPartidaEntity record)
        {          
            return servicio.crearPartida(record);
        }

        [Route("partidasActivas")]
        [HttpGet]
        public string getPartidasActivas()
        {
            var json = JsonConvert.SerializeObject(servicio.partidasActivas());

            return json;
        }

        [Route("crearJugador")]
        [HttpPost]
        public int crearJugador([FromBody] infoJugadorEntity record)
        {
            return servicio.crearJugador(record);
        }

        [Route("iniciarSesion")]
        [HttpGet]
        public int iniciarSesion([FromBody] infoJugadorEntity record)
        {
            return servicio.inciarSesion(record);
        }

        [Route("parametrosPartida")]
        [HttpGet]
        public string obtenerParametrosPartida(int id_Partida)
        {
            var json = JsonConvert.SerializeObject(servicio.obtenerParametrosPartida(id_Partida));

            return json;
        }

        [Route("comprobarFinallizacion")]
        [HttpGet]
        public string comprobarFinalizacion(int id_Partida)
        {
            var json = JsonConvert.SerializeObject(servicio.comprobarFinalizacion(id_Partida));

            return json;
        }

    }
}