using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WindowsFormsDAMapp.Entidades
{
    public class infoTropasEntity
    {

        public int id_Tropa { get; set; }

        public string nombre { get; set; }

        public int potencia { get; set; }

        public TimeSpan tiempoReclutamiento { get; set; }

        public int poblacion { get; set; }

    }
}