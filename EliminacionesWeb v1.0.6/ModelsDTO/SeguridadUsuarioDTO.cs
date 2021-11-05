using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class SeguridadUsuarioDTO
    {
        public string UsuLegajo { get; set; }
        public string UsuNombre { get; set; }
        public string UsuPerfil { get; set; }
        public int SecCodigo { get; set; }
        public string SecDescripcion { get; set; }
        public string GrupoAD { get; set; }

    }
}
