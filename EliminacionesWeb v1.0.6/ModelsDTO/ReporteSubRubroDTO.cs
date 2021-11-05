using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class ReporteSubRubroDTO
    {
        public int SubrubId { get; set; }
        public string SubrubDescripcion { get; set; }
        public List<EliminacionesDTO> ListaEliminaciones { get; set; }
        public decimal TotalSubRubros { get; set; }

    }
}
