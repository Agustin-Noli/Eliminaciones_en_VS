using System;
using System.Collections.Generic;

namespace EliminacionesWeb.Models
{
    public partial class ImportPlanillas
    {
        public int EmpCodigo { get; set; }
        public string Periodo { get; set; }
        public string NombrePlanilla { get; set; }
        public string Porcentaje { get; set; }
        public int SecCodigo { get; set; }
        public int? MonCodigo { get; set; }
        public string Importacion { get; set; }
    }
}
