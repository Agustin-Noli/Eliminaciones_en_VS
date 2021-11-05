using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class PlanillasCargadasDTO
    {
        public int EmpCodigo { get; set; }
        public string EmpDescripcion { get; set; }
        public string Periodo { get; set; }
        public int Porcentaje { get; set; }
        public int? MonCodigo { get; set; }
        public string MonDescripcion { get; set; }
        public string Importacion { get; set; }
        public string NombrePlanilla { get; set; }
        public int SecCodigo { get; set; }
    }
}
