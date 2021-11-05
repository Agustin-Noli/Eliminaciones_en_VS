using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EliminacionesWeb.Models;
using EliminacionesWeb.ModelsDTO;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using System.DirectoryServices.AccountManagement;
//using System.Security.Claims;

namespace EliminacionesWeb.Controllers
{
    [Route("Eliminaciones/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;
        private readonly IConfiguration Configuration;

        public SeguridadController(EliminacionesContext_Custom context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: Eliminaciones/Seguridad/GetUsuarioAD
        /// <summary>
        /// Verifica si el usuario tiene acceso a la aplicacion y devuelve sus datos
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsuarioAD")]
        public async Task<ActionResult<SeguridadUsuarioDTO>> GetUsuarioAD()
        {
            //Conexion AD
            try
            {

                // gruposAD admitidos en la aplicacion
                string NombreBalGrupoAD = Configuration["NombreBalGrupoAD"];    //"CMRMELIBAL";
                string NombreGFGGrupoAD = Configuration["NombreGFGGrupoAD"];    //"CMRMELIGFG";
                string Legajo = HttpContext.User.Identity.Name.Split("\\")[1];

                if (Legajo == null || Legajo == "")
                    return StatusCode(403, "Usuario: " + User.Identity.Name + "DETALLE: No existe Legajo");

                //bool EsBalances = PerteneceAlGrupoAD(Legajo, NombreBalGrupoAD);
                //bool EsGFG = PerteneceAlGrupoAD(Legajo, NombreGFGGrupoAD);

                bool EsBalances = User.IsInRole(NombreBalGrupoAD);
                bool EsGFG = User.IsInRole(NombreGFGGrupoAD);


                //if (EsBalances || EsGFG)
                if (EsBalances || EsGFG || Legajo.ToUpper() == "L0697451" || Legajo.ToUpper() == "L0694401" || Legajo.ToUpper() == "L0683973" ||
                    Legajo.ToUpper() == "L0301922" || Legajo.ToUpper() == "L0293008" || Legajo.ToUpper() == "L0692727" || Legajo.ToUpper() == "L0281417")
                {
                    int _secCodigo;

                    // quitar harcodeo cuando esten cargados los accesos desde seguridad informatica con los gruposAD corespondientes
                    if (Legajo.ToUpper() == "L0697451" || Legajo.ToUpper() == "L0694401" || Legajo.ToUpper() == "L0683973" || Legajo.ToUpper() == "L0301922" 
                        || Legajo.ToUpper() == "L0692727")
                        _secCodigo = 2;
                    else if (Legajo.ToUpper() == "L0293008" || Legajo.ToUpper() == "L0281417")
                        _secCodigo = 1;
                    else
                        _secCodigo = (EsBalances == true ? 2 : 1);

                    IQueryable<SeguridadUsuarioDTO> usuarioAD = (from usu in _context.Usuarios
                                                                 where usu.UsuLegajo == Legajo && usu.SecCodigo == _secCodigo
                                                                 select new SeguridadUsuarioDTO
                                                                 {
                                                                     UsuLegajo = Legajo,
                                                                     UsuNombre = GetNombreUsuarioAD(Legajo),
                                                                     UsuPerfil = (from per in _context.Perfiles where usu.PerCodigo == per.PerCodigo select per.PerDescripcion).First().ToString(),
                                                                     SecCodigo = _secCodigo,
                                                                     SecDescripcion = (from sec in _context.Sectores where sec.SecCodigo == _secCodigo select sec.SecDescripcion).First().ToString(),
                                                                     GrupoAD = (_secCodigo == 1 ? NombreGFGGrupoAD : NombreBalGrupoAD)
                                                                 }).Distinct();

                    return await usuarioAD.SingleOrDefaultAsync();
                }
                else // NO TIENE ACCESO
                {
                    return StatusCode(403, "Usuario: " + User.Identity.Name + "DETALLE: Acceso denegado"); // NoContent();
                }
            }
            catch (Exception ex)
            {
                return Conflict("Usuario: " + User.Identity.Name + "ERROR:  " + ex.Message + "DETALLE: " + ex.StackTrace);
            }
        }

        //private bool PerteneceAlGrupoAD(string Legajo, string NombreGrupoAD)
        //{
        //    using (var context = new PrincipalContext(ContextType.Domain, "BGCMZ"))
        //    {
        //        var grupo = GroupPrincipal.FindByIdentity(context, NombreGrupoAD);
        //        if (grupo != null)
        //        {
        //            using (UserPrincipal user = UserPrincipal.FindByIdentity(context, Legajo))
        //            {
        //                return user.IsMemberOf(grupo);
        //            }
        //        }

        //        return false;
        //    }
        //}

        private string GetNombreUsuarioAD(string Legajo)
        {
            string ambiente = Configuration["Ambiente"];

            if (ambiente == "PROD")
                return User.Identity.Name.Split("\\")[1].TrimEnd();

            string dominio = (ambiente == "DESA") ? "BGCMZ" : "HBGCMZ";

            using (var context = new PrincipalContext(ContextType.Domain, dominio))
            {
                using (UserPrincipal user = UserPrincipal.FindByIdentity(context, Legajo))
                {
                    return (user != null) ? user.DisplayName : string.Empty;
                }
            }
        }

    }
}
