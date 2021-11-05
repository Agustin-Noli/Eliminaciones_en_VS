using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class AjustesDTO
    {
        public int EmpCodigo { get; set; }
        public string EmpDescripcion { get; set; }
        public string Periodo { get; set; }
        public string RubCodigo { get; set; }
        public string RubDescripcion { get; set; }
        public int EmpCodigoContraparte { get; set; }
        public string EmpDescripcionContraparte { get; set; }
        public decimal? AjuSaldo { get; set; }
        public decimal? AjuSaldoPromedio { get; set; }
        public int SecCodigo { get; set; }
        public string SecDescripcion { get; set; }
    }
}
