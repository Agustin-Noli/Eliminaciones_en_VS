using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class HistorialDTO
    {
        public int HistId { get; set; }
        public string UsuLegajo { get; set; }
        public string UsuNombre { get; set; }
        public DateTime? FechaHora { get; set; }
        public string Periodo { get; set; }
        public string Accion { get; set; }
        public string Mensaje { get; set; }
        public int SecCodigo { get; set; }
        public string SecDescripcion { get; set; }
    }
}