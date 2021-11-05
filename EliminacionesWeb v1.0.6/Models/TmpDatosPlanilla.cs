using System;
using System.Collections.Generic;

namespace EliminacionesWeb.Models
{
    public partial class TmpDatosPlanilla
    {
        public int EmpCodigo { get; set; }
        public string Periodo { get; set; }
        public string RubCodigo { get; set; }
        public string Concepto { get; set; }
        public int EmpCodigoContraparte { get; set; }
        public int? MonCodigo { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? SaldoPromedio { get; set; }
        public byte? Ind { get; set; }
        public int? Pond { get; set; }
        public string Exposicion { get; set; }
        public int SecCodigo { get; set; }
    }
}
