using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class txtOutputDTO
    {
        // scenario	version	año	periodo	intercompany	divisa	empresa	saldos	rubro	data

        public string scenario { get; set; }
        public string version { get; set; }
        public string año { get; set; }
        public string periodo { get; set; }
        public string intercompany { get; set; }
        public string divisa { get; set; }
        public string empresa { get; set; }
        public string saldos { get; set; }
        public string rubro { get; set; }
        public string data { get; set; }
    }
}
