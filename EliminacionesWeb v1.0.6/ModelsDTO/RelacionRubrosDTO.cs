using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class RelacionRubrosDTO
    {
        public string RubcabCodigo { get; set; }
        public string RubcabDescripcion { get; set; }
        public string GranrubCodigo { get; set; }
        public string GranrubDescripcion { get; set; }
        public int SubrubId { get; set; }
        public string SubrubDescripcion { get; set; }
        public string RubCodigo { get; set; }
        public string RubroDescripcion { get; set; }

    }
}
