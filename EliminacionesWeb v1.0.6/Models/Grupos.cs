using System;
using System.Collections.Generic;

namespace EliminacionesWeb.Models
{
    public partial class Grupos
    {
        public int GrupoId { get; set; }
        public string GrupoNombre { get; set; }
        public string GrupoDescripcion { get; set; }
        public int SecCodigo { get; set; }
        public string Activo { get; set; }
    }
}
