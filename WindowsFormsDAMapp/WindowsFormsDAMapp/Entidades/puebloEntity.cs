using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WindowsFormsDAMapp.Entidades
{
    public class puebloEntity
    {
        public int id_Pueblo { get; set; }

        public string  propietario { get; set; }

        public int poblacionRestante { get; set; }

        public int arqueros { get; set; }

        public int ballesteros { get; set; }

        public int piqueros { get; set; }

        public int caballeros { get; set; }

        public int paladines { get; set; }

        public string coordenadas { get; set; }

    }
}