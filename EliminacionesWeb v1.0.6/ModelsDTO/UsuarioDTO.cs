using System;
using System.Collections.Generic;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.ModelsDTO
{
    public partial class UsuarioDTO
    {
        public string UsuLegajo { get; set; }
        public string UsuNombre { get; set; }
        public int PerCodigo { get; set; }
        public string UsuPerfil { get; set; }
        public DateTime? UsuFecalta { get; set; }
        public int SecCodigo { get; set; }
        public string Activo { get; set; }
    }
}