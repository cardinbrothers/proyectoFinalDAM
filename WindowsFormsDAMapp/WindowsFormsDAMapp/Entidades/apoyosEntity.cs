using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WindowsFormsDAMapp.Entidades
{
    public class apoyosEntity
    {
        public int id_Apoyo { get; set; }

        public int puebloOrigen { get; set; }

        public int puebloDestino { get; set; }

        public int arqueros { get; set; }

        public int ballesteros { get; set; }

        public DateTime horaFin { get; set; }

    }
}