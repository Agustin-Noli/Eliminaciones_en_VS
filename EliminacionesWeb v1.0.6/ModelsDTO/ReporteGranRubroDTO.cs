using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class ReporteGranRubroDTO
    {
        public string GranrubCodigo { get; set; }
        public string GranrubDescripcion { get; set; }
        public string RubcabCodigo { get; set; }
        public List<ReporteSubRubroDTO> ListaSubRubros { get; set; }
        public decimal TotalGranRubro { get; set; }
        
    }
}
