using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class ReporteSelectSubRubroDTO
    {
        public string rubro { get; set; }
        public string origen { get; set; }
        public string contraparte { get; set; }
        //public string concepto { get; set; }
        public Int32 bcra { get; set; }
        public string moneda { get; set; }
        public decimal saldo { get; set; }
        public decimal saldoPromedio { get; set; }


    }
}
