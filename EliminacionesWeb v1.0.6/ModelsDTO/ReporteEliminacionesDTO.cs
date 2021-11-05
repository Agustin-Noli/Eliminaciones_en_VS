using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class ReporteEliminacionesDTO
    {
        public int EmpCodigo { get; set; }
        public string EmpDescripcion { get; set; }
        public string Periodo { get; set; }
        public int EmpCodigoContraparte { get; set; }
        public string EmpDescripcionContraparte { get; set; }
        public string RubCodigo { get; set; }
        public string RubDescripcion { get; set; }
        public decimal? EliSaldo { get; set; }
        public decimal? EliSaldoPromedio { get; set; }
        public decimal? EmpPorcentaje { get; set; }
        public int? MonCodigo { get; set; }
        public string MonDescripcion { get; set; }
        public int SecCodigo { get; set; }
        public string SecDescripcion { get; set; }
        public string EsAjuste { get; set; }
        public string RubcabCodigo { get; set; }
        public string RubcabDescripcion { get; set; }
        public string GranrubCodigo { get; set; }
        public string GranrubDescripcion { get; set; }
        public int SubrubId { get; set; }
        public string SubrubDescripcion { get; set; }
    }
}