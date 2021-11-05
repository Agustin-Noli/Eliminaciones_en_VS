using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class DatosPlanillaDTO
    {
        public int EmpCodigo { get; set; }
        public string EmpDescripcion { get; set; }
        public string Periodo { get; set; }
        public string RubCodigo { get; set; }
        public string RubDescripcion { get; set; }
        public string Concepto { get; set; }
        public int EmpCodigoContraparte { get; set; }
        public string EmpDescripcionContraparte { get; set; }
        public int? MonCodigo { get; set; }
        public string MonDescripcion { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? SaldoPromedio { get; set; }
        public byte? Ind { get; set; }
        public int? Pond { get; set; }
        public string Exposicion { get; set; }
        public int SecCodigo { get; set; }
        public string SecDescripcion { get; set; }

    }
}
