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
    [RoutePrefix("api/Pueblo")]
    public class PueblosController : ApiController
    {
        servicioManejoPueblos servicio = new servicioManejoPueblos();

        [Route("obtenerPueblo")]
        [HttpGet]
        public string obtenerPueblo(int id_Pueblo)
        {
            var json = JsonConvert.SerializeObject(servicio.obtenerPueblo(id_Pueblo));

            return json;
        }

        [Route("obtenerListaPueblos")]
        [HttpGet]
        public string obtenerListaPueblos(string propietario)
        {
            var json = JsonConvert.SerializeObject(servicio.obtenerListaPueblos(propietario));

            return json;
        }

        [Route("obtenerCoords")]
        [HttpGet]
        public List<string> obtenerCoords(int id_Partida)
        {
            return servicio.obtenerCoordsPartida(id_Partida);
        }

        [Route("obtenerPotencia")]
        [HttpGet]
        public int obtenerPotencia(string nombreJugador)
        {
            return servicio.obtenerPotenciaJugador(nombreJugador);
        }

        [Route("obtenerDefReal")]
        [HttpGet]
        public string obtenerDefReal(int id_Pueblo)
        {
            var json = JsonConvert.SerializeObject(servicio.obtenerDefRealPueblo(id_Pueblo));

            return json;
        }

        [Route("obtenerDistancia")]
        [HttpGet]
        public TimeSpan obtenerDistancia(string coordenada1, string coordenada2, int id_Partida)
        {
            return servicio.obtenerDistancia(coordenada1, coordenada2, id_Partida);
        }

        [Route("obtenerId")]
        [HttpGet]
        public int obtenerId(string coordenada, int id_Partida)
        {
            return servicio.obtenerId(coordenada, id_Partida);
        }

    }
}
