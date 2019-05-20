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
    [RoutePrefix("api/Reclutamiento")]
    public class ReclutamientoController : ApiController
    {
        servicioReclutamientoTropas servicio = new servicioReclutamientoTropas();

        [Route("reclutarTropas")]
        [HttpPost]
        public string reclutarTropas(int idPueblo, int idTropa, int cantidad)
        {
            var json = JsonConvert.SerializeObject(servicio.reclutarTropas(idPueblo, idTropa, cantidad));

            return json;
        }

        [Route("obtenerOrdenes")]
        [HttpGet]
        public string obtenerOrdenes(int idPueblo)
        {
            var json = JsonConvert.SerializeObject(servicio.obtenerOrdenesActivas(idPueblo));

            return json;
        }

        [Route("cancelar")]
        [HttpPost]
        public int cancelar(int idOrden)
        {
            return servicio.cancelarReclutamiento(idOrden);
        }

    }
}
