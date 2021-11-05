using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EliminacionesWeb.Models;

namespace EliminacionesWeb.Controllers
{
    [Route("Eliminaciones/[controller]")]
    [ApiController]
    public class GrupoEmpresasController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;

        public GrupoEmpresasController(EliminacionesContext_Custom context)
        {
            _context = context;
        }

        // GET: Eliminaciones/GrupoEmpresas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GrupoEmpresas>>> GetGrupoEmpresas()
        {
            return await _context.GrupoEmpresas.ToListAsync();
        }

        // GET: Eliminaciones/GetEmpresasByGrupo
        /// <summary>
        /// Obtiene las empresas asociadas al grupo indicado
        /// </summary>
        /// <param name="Grupo_Id"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("GetEmpresasByGrupo")]
        public async Task<ActionResult<IEnumerable<Empresas>>> GetEmpresasByGrupo([FromQuery] int Grupo_Id, [FromQuery] int Sec_Codigo)
        {
            IQueryable<Empresas> results = (from _gemp in _context.GrupoEmpresas
                                            join _emp in _context.Empresas
                                                on new { _gemp.EmpCodigo, _gemp.SecCodigo } equals new { _emp.EmpCodigo, _emp.SecCodigo }
                                            where _gemp.GrupoId == Grupo_Id && _gemp.SecCodigo == Sec_Codigo
                                            select _emp);

            if (results == null)
                return NoContent();

            return await results.ToListAsync();
        }

        // POST: Eliminaciones/GrupoEmpresas
        /// <summary>
        /// Inserta una nueva asociacion entre grupo y empresa segun el secCodigo
        /// </summary>
        /// <param name="GrupoEmp"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<GrupoEmpresas>> InsertGrupoEmpresas([FromBody] GrupoEmpresas GrupoEmp)
        {
            _context.GrupoEmpresas.Add(GrupoEmp);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GrupoEmpresasExists(GrupoEmp))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return GrupoEmp;
        }

        // DELETE: Eliminaciones/GrupoEmpresas/5
        /// <summary>
        /// Elimina la asociacion entre grupo y empresa segun el secCodigo 
        /// </summary>
        /// <param name="GrupoEmp"></param>
        /// <returns></returns>
        [HttpPost("DeleteGrupoEmpresas")]
        public async Task<ActionResult<GrupoEmpresas>> DeleteGrupoEmpresas([FromBody] GrupoEmpresas GrupoEmp)
        {
            var grupoEmpresas = await _context.GrupoEmpresas.Where(ge => ge.GrupoId == GrupoEmp.GrupoId && ge.EmpCodigo == GrupoEmp.EmpCodigo && ge.SecCodigo == GrupoEmp.SecCodigo).SingleOrDefaultAsync();
            if (grupoEmpresas == null)
            {
                return NoContent();
            }

            _context.GrupoEmpresas.Remove(grupoEmpresas);
            await _context.SaveChangesAsync();

            return grupoEmpresas;
        }

        private bool GrupoEmpresasExists(GrupoEmpresas grupoEmp)
        {
            return _context.GrupoEmpresas.Any(ge => ge.GrupoId == grupoEmp.GrupoId && ge.EmpCodigo == grupoEmp.EmpCodigo && ge.SecCodigo == grupoEmp.SecCodigo);
        }
    }
}
