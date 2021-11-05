using System;
using System.Collections.Generic;

namespace EliminacionesWeb.Models
{
    public partial class Historial
    {
        public int HistId { get; set; }
        public string UsuLegajo { get; set; }
        public DateTime? FechaHora { get; set; }
        public string Periodo { get; set; }
        public string Accion { get; set; }
        public string Mensaje { get; set; }
        public int SecCodigo { get; set; }
    }
}
