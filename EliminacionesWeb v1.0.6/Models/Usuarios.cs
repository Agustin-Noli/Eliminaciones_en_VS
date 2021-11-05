using System;
using System.Collections.Generic;

namespace EliminacionesWeb.Models
{
    public partial class Usuarios
    {
        public string UsuLegajo { get; set; }
        public string UsuNombre { get; set; }
        public int PerCodigo { get; set; }
        public DateTime? UsuFecalta { get; set; }
        public int SecCodigo { get; set; }
        public string Activo { get; set; }
    }
}
