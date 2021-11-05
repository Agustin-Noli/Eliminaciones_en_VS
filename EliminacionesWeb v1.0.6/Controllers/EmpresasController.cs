using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EliminacionesWeb.Models;
using EliminacionesWeb.Helpers;

namespace EliminacionesWeb.Controllers
{
    [System.Web.Http.Cors.EnableCors(origins: "http://dco16liis51:9082", headers: "*", methods: "GET,POST,OPTIONS,DELETE,PUT")]
    [Route("Eliminaciones/[controller]")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;

        public EmpresasController(EliminacionesContext_Custom context)
        {
            _context = context;
        }

        // GET: Eliminaciones/Empresas/1
        /// <summary>
        /// Obtiene todas las Empresas
        /// </summary>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("{Sec_Codigo}")]
        public async Task<ActionResult<IEnumerable<Empresas>>> GetEmpresas(int Sec_Codigo)
        {
            return await _context.Empresas.Where(id => id.SecCodigo == Sec_Codigo && id.Activo == "S").ToListAsync();
        }

        // GET: Eliminaciones/GetEmpresa
        /// <summary>
        /// Obtiene la empresa solicitada por parametro
        /// </summary>
        /// <param name="Emp_Codigo"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        //[HttpGet("GetEmpresa/{Emp_Codigo}/{Sec_Codigo}")]
        [HttpGet("GetEmpresa")]
        public async Task<ActionResult<Empresas>> GetEmpresa([FromQuery] int Emp_Codigo, [FromQuery] int Sec_Codigo)
        {
            var empresas = await _context.Empresas.Where(id => id.EmpCodigo == Convert.ToInt32(Emp_Codigo) && id.SecCodigo == Convert.ToInt32(Sec_Codigo) && id.Activo == "S").SingleOrDefaultAsync();

            if (empresas == null)
                return NoContent();

            return empresas;
        }

        // GET: Eliminaciones/GetEmpresasByPeriodo
        /// <summary>
        /// Obtiene las empresas cargadas en el periodo indicado
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("GetEmpresasByPeriodo")]
        public async Task<ActionResult<IEnumerable<Empresas>>> GetEmpresasByPeriodo([FromQuery] string Periodo, [FromQuery] int Sec_Codigo)
        {
            IQueryable<Empresas> results = (from _emp in _context.Empresas
                                            join _imp in _context.ImportPlanillas
                                                on _emp.EmpCodigo equals _imp.EmpCodigo
                                            where _imp.Periodo == Periodo && _imp.SecCodigo == Sec_Codigo && _emp.Activo == "S"
                                            select _emp);

            if (results == null)
                return NoContent();

            return await results.ToListAsync();
        }

        // GET: Eliminaciones/GetEmpresasByGrupo
        /// <summary>
        /// Obtiene las empresas asociadas al grupo indicado
        /// </summary>
        /// <param name="Grupo_Id"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        //[HttpGet("GetEmpresasByGrupo")]
        //public async Task<ActionResult<IEnumerable<Empresas>>> GetEmpresasByGrupo([FromQuery] int Grupo_Id, [FromQuery] int Sec_Codigo)
        //{
        //    IQueryable<Empresas> results = (from _gemp in _context.GrupoEmpresas
        //                                    join _emp in _context.Empresas
        //                                        on new { _gemp.EmpCodigo, _gemp.SecCodigo} equals new { _emp.EmpCodigo, _emp.SecCodigo} 
        //                                    where _gemp.GrupoId == Grupo_Id && _gemp.SecCodigo == Sec_Codigo
        //                                    select _emp);

        //    if (results == null)
        //        return NoContent();

        //    return await results.ToListAsync();
        //}

        // PUT: Eliminaciones/Empresas/5
        /// <summary>
        /// Actualiza la empresa indicada por parametro
        /// </summary>
        /// <param name="Emp_Codigo"></param>
        /// <param name="empresas"></param>
        /// <returns></returns>
        [HttpPut("{Emp_Codigo}")]
        public async Task<IActionResult> UpdateEmpresas([FromRoute] int Emp_Codigo, [FromBody] Empresas empresas)
        {
            if (Emp_Codigo != empresas.EmpCodigo)
                return BadRequest();

            empresas.Activo = "S";
            _context.Entry(empresas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                Historial log = new Historial
                {
                    UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                    Periodo = string.Empty,
                    FechaHora = DateTime.Now,
                    Accion = "Modificacion de Empresa",
                    Mensaje = "Se modifico la empresa " + empresas.EmpDescripcion,
                    SecCodigo = empresas.SecCodigo
                };
                _context.Historial.Add(log);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresasExists(Emp_Codigo, empresas.SecCodigo))
                    return NotFound();
                else
                    throw;
            }
            // modificar para que devuelva status 200
            return NoContent();
        }

        // POST: Eliminaciones/Empresas
        /// <summary>
        /// Crea una nueva empresa
        /// </summary>
        /// <param name="empresas"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Empresas>> CreateEmpresas([FromBody] Empresas empresas)
        {
            empresas.Activo = "S";
            _context.Empresas.Add(empresas);
            await _context.SaveChangesAsync();

            Historial log = new Historial
            {
                UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                Periodo = string.Empty,
                FechaHora = DateTime.Now,
                Accion = "Alta de Empresa",
                Mensaje = "Se creo la empresa " + empresas.EmpDescripcion,
                SecCodigo = empresas.SecCodigo
            };
            _context.Historial.Add(log);
            await _context.SaveChangesAsync();


            //return CreatedAtAction("GetEmpresas", new { id = empresas.EmpCodigo }, empresas);
            return empresas;
        }

        // DELETE: Eliminaciones/Empresas/5
        /// <summary>
        /// Elimina la empresa indicada por parametro
        /// </summary>
        /// <param name="Emp_Codigo"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<Empresas>> DeleteEmpresas([FromQuery] int Emp_Codigo, [FromQuery] int Sec_Codigo)
        {
            var empresas = await _context.Empresas.Where(e => e.EmpCodigo == Emp_Codigo && e.SecCodigo == Sec_Codigo && e.Activo == "S").SingleOrDefaultAsync();
            //var empresas = await _context.Empresas.FindAsync( new { Emp_Codigo, Sec_Codigo });

            if (empresas == null)
                return NoContent();

            //_context.Empresas.Remove(empresas);
            empresas.Activo = "N";
            _context.Entry(empresas).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            Historial log = new Historial
            {
                UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                Periodo = string.Empty,
                FechaHora = DateTime.Now,
                Accion = "Eliminacion de Empresa",
                Mensaje = "Se elimino la empresa " + empresas.EmpDescripcion,
                SecCodigo = empresas.SecCodigo
            };
            _context.Historial.Add(log);
            await _context.SaveChangesAsync();

            return empresas;
        }

        private bool EmpresasExists(int Emp_Codigo, int Sec_Codigo)
        {
            return _context.Empresas.Any(e => e.EmpCodigo == Emp_Codigo && e.SecCodigo == Sec_Codigo);
        }


    }
}
