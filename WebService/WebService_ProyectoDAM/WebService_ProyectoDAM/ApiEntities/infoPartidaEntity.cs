﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService_ProyectoDAM.ApiEntities
{
    public class infoPartidaEntity
    {
        public int id_Partida { get; set; }

        public string modalidad { get; set; }

        public int velocidad { get; set; }

        public TimeSpan duracion { get; set; }

        public int limiteJugadores { get; set; }

        public int limitePoblacion { get; set; }

        public DateTime fecjaInicio { get; set; }


    }
}