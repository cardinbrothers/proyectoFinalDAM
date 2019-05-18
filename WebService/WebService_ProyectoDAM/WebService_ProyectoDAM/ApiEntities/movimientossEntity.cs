using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService_ProyectoDAM.ApiEntities
{
    public class movimientossEntity
    {
        public int id_Apoyo { get; set; }

        public int puebloOrigen { get; set; }

        public int puebloDestino { get; set; }

        public string tipoMovimiento { get; set; }

        public DateTime horaLlegda { get; set; }

        public TimeSpan duracion { get; set; }

        public int arqueros { get; set; }

        public int ballesteros { get; set; }

        public int piqueros { get; set; }

        public int caballeros { get; set; }

        public int paladines { get; set; }

    }
}