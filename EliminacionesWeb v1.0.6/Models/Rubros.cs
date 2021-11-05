using System;
using System.Collections.Generic;

namespace EliminacionesWeb.Models
{
    public partial class Rubros
    {
        public string RubCodigo { get; set; }
        public string RubDescripcion { get; set; }
        public string RubSigno { get; set; }
        public byte? RubNivel { get; set; }
        public string RubOrden { get; set; }
        public string EsBcra { get; set; }
    }
}
