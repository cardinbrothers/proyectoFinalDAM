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
    }
}
