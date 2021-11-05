using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class ReporteCabeceraDTO
    {
        public string RubcabCodigo { get; set; }
        public string RubcabDescripcion { get; set; }
        public List<ReporteGranRubroDTO> ListaGrandesRubros { get; set; }
        public decimal TotalCabecera { get; set; }

    }
}
