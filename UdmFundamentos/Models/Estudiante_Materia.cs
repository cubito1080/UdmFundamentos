//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UdmFundamentos.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estudiante_Materia
    {
        public int id { get; set; }
        public Nullable<int> estudiante_id { get; set; }
        public Nullable<int> materia_id { get; set; }
        public Nullable<decimal> nota { get; set; }
        public Nullable<int> semestre_id { get; set; }
    
        public virtual Estudiante Estudiante { get; set; }
        public virtual Materia Materia { get; set; }
        public virtual Semestre Semestre { get; set; }
    }
}