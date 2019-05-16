using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService_ProyectoDAM.ApiEntities
{
    public class ordenReclutamientoEntity
    {
        public int id_Orden { get; set; }

        public int id_Tropa { get; set; }

        public int cantidad { get; set; }

        public DateTime horaFin { get; set; }

        public int error { get; set; }
    }
}