using System;
using System.Collections.Generic;

namespace EliminacionesWeb.Models
{
    public partial class Empresas
    {
        public int EmpCodigo { get; set; }
        public string EmpDescripcion { get; set; }
        public string EmpIntercompany { get; set; }
        public int EmpPorcentaje { get; set; }
        public int SecCodigo { get; set; }
        public string Activo { get; set; }
    }
}
