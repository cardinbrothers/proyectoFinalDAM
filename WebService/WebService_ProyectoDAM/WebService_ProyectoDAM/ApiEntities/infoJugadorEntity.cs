using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService_ProyectoDAM.ApiEntities
{
    public class infoJugadorEntity
    {
        public string nombreUsuario { get; set; }

        public string contraseña { get; set; }

        public int id_partida { get; set; }
    }
}