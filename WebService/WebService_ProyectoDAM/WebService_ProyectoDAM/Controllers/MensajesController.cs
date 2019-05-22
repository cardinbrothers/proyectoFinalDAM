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
    [RoutePrefix("api/Mensaje")]
    public class MensajesController : ApiController
    {
        servicioMensajeria servicio = new servicioMensajeria();

        [Route("mandarMensaje")]
        [HttpPost]
        public int mandarMensaje([FromBody] mensajesEntity record)
        {
            return servicio.mandarMensajenuevo(record);
        }

        [Route("obtenerMensajesP")]
        [HttpGet]
        public List<mensajesEntity> obtenerMensajesP(string nombreUsuario)
        {
            //var json = JsonConvert.SerializeObject(servicio.listaMensajesPrinciapales(nombreUsuario));

            return servicio.listaMensajesPrinciapales(nombreUsuario);
        }

        [Route("obtenerMensajesS")]
        [HttpGet]
        public List<mensajesEntity> obtenerMensajesS(int id_Mensaje)
        {
            //var json = JsonConvert.SerializeObject(servicio.listaMensajesSecundarios(id_Mensaje));

            return servicio.listaMensajesSecundarios(id_Mensaje);
        }

        [Route("responderMensaje")]
        [HttpPost]
        public int responderMensaje([FromBody] mensajesEntity record)
        {
            return servicio.responderMensaje(record);
        }
    }
}
