//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebService_ProyectoDAM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Partida
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Partida()
        {
            this.Jugador = new HashSet<Jugador>();
        }
    
        public int id_Partida { get; set; }
        public string Modalidad { get; set; }
        public int Velocidad { get; set; }
        public System.TimeSpan Duracion { get; set; }
        public int limiteJugadores { get; set; }
        public int limitePoblacion { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public bool activo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Jugador> Jugador { get; set; }
    }
}
