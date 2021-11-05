using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class ImportPlanillasDTO 
    {
        public int EmpCodigo { get; set; }
        public string EmpDescripcion { get; set; }
        public string Periodo { get; set; }
        public string NombrePlanilla { get; set; }
        //public int Grupo { get; set; }
        //public string GrupoDescripcion { get; set; }
        public string Porcentaje { get; set; }
        public int SecCodigo { get; set; }
        public int? MonCodigo { get; set; }
        public string Importacion { get; set; }

    }
}
