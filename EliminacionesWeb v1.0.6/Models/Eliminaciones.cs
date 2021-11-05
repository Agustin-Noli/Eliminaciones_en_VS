using System;
using System.Collections.Generic;

namespace EliminacionesWeb.Models
{
    public partial class Eliminaciones
    {
        public int EmpCodigo { get; set; }
        public string Periodo { get; set; }
        public int EmpCodigoContraparte { get; set; }
        public string RubCodigo { get; set; }
        public decimal? EliSaldo { get; set; }
        public decimal? EliSaldoPromedio { get; set; }
        public decimal? EmpPorcentaje { get; set; }
        public int? MonCodigo { get; set; }
        public int SecCodigo { get; set; }
        public string EsAjuste { get; set; }
    }
}
