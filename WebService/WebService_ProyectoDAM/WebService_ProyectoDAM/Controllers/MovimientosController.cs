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
    [RoutePrefix("api/Movimientos")]
    public class MovimientosController : ApiController
    {
        servicioManejoMovimientos servicio = new servicioManejoMovimientos();

        [Route("realizarMovimiento")]
        [HttpPost]
        public void realizarMovimientos([FromBody] movimientosEntity record)
        {
            servicio.realizarMovimiento(record);
        }

        [Route("obtenerMovimientos")]
        [HttpGet]
        public List<movimientosEntity> obtenerMovimientos(int id_pueblo)
        {
            //var json = JsonConvert.SerializeObject(servicio.obtenerMovimientosActivos(id_pueblo));

            return servicio.obtenerMovimientosActivos(id_pueblo);
        }

        [Route("obtenerVencedor")]
        [HttpGet]
        public int obtenerVencedor(int id_movimiento)
        {
            int result = servicio.obtenedorVencedorBatalla(id_movimiento);

            return result;
        }
    }
}
