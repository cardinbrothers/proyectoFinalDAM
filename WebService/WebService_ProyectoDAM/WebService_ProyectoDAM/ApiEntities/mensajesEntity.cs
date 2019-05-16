using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService_ProyectoDAM.ApiEntities
{
    public class mensajesEntity
    {
        public int id_Mensaje { get; set; }

        public string usuarioEmisor { get; set; }

        public string usuarioReceptor { get; set; }

        public DateTime fecha { get; set; }

        public string asunto { get; set; }

        public string contenido { get; set; }

        public int mensajePadre { get; set; }


    }
}