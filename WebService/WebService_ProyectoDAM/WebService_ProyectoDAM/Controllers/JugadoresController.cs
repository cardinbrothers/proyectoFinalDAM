using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService_ProyectoDAM.ApiEntities;
using WebService_ProyectoDAM.Servicios;
using Newtonsoft.Json;

namespace WebService_ProyectoDAM.Controllers
{
    [RoutePrefix("api/Jugadores")]
    public class JugadoresController : ApiController
    {
        servicioManejoJugadores servicio = new servicioManejoJugadores();

        [Route("obtenerJugadores")]
        [HttpGet]
        public List<infoJugadorEntity> obtenerJugadores(int id_Partida)
        {
            //var json = JsonConvert.SerializeObject(servicio.obtenerJugadores(id_Partida));

            return servicio.obtenerJugadores(id_Partida);
        }

        [Route("obtenerClasificacion")]
        [HttpGet]
        public List<potenciaJugadorEntity> obtenerClasificacion(int id_Partida)
        {
            //var json = JsonConvert.SerializeObject(servicio.obtenerClasificacion(id_Partida));

            return servicio.obtenerClasificacion(id_Partida);
        }

    }
}
