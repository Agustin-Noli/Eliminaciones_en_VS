using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EliminacionesWeb.Models;
using EliminacionesWeb.ModelsDTO;

namespace EliminacionesWeb.Controllers
{
    [System.Web.Http.Cors.EnableCors(origins: "http://dco16liis51:9082", headers: "*", methods: "GET,POST,OPTIONS,DELETE,PUT")]
    [Route("Eliminaciones/[controller]")]
    [ApiController]
    public class DatosPlanillasController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;

        public DatosPlanillasController(EliminacionesContext_Custom context)
        {
            _context = context;
        }

        // GET: Eliminaciones/DatosPlanillas
        /// <summary>
        /// Obtiene todos los datos de las planillas cargadas segun el Periodo enviado por parametro
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="GrupoId"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("GetDatosPlanillaByPeriodo")]
        public async Task<ActionResult<IEnumerable<DatosPlanillaDTO>>> GetDatosPlanillaByPeriodo([FromQuery]string Periodo, [FromQuery]int GrupoId, [FromQuery]int Sec_Codigo)
        {
            var ListEmpbyGrupo = (from gemp in _context.GrupoEmpresas where gemp.GrupoId == GrupoId && gemp.SecCodigo == Sec_Codigo select gemp.EmpCodigo).ToList();

            IQueryable<DatosPlanillaDTO> results = (from DP in _context.DatosPlanilla
                                                    where DP.Periodo == Periodo && DP.SecCodigo == Sec_Codigo && ListEmpbyGrupo.Contains(DP.EmpCodigo)
                                                    select new DatosPlanillaDTO
                                                    {
                                                        EmpCodigo = DP.EmpCodigo,
                                                        EmpDescripcion = (from emp in _context.Empresas where emp.EmpCodigo == DP.EmpCodigo select emp.EmpDescripcion).First().ToString(),
                                                        Periodo = DP.Periodo,
                                                        RubCodigo = DP.RubCodigo,
                                                        RubDescripcion = (from rub in _context.Rubros where rub.RubCodigo == DP.RubCodigo select rub.RubDescripcion).First().ToString(),
                                                        Concepto = DP.Concepto,
                                                        EmpCodigoContraparte = DP.EmpCodigoContraparte,
                                                        EmpDescripcionContraparte = ((from empC in _context.Empresas where empC.EmpCodigo == DP.EmpCodigoContraparte select empC.EmpIntercompany).First().Count() > 0) ?
                                                        (from empC in _context.Empresas where empC.EmpCodigo == DP.EmpCodigoContraparte select empC.EmpIntercompany).First().ToString() : null,
                                                        MonCodigo = DP.MonCodigo,
                                                        MonDescripcion = (from mon in _context.Moneda where mon.MonCodigo == DP.MonCodigo select mon.MonDescripcion).First().ToString(),
                                                        Saldo = DP.Saldo,
                                                        SaldoPromedio = DP.SaldoPromedio,
                                                        Ind = DP.Ind,
                                                        Pond = DP.Pond,
                                                        Exposicion = DP.Exposicion,
                                                        SecCodigo = DP.SecCodigo,
                                                        SecDescripcion = (from sec in _context.Sectores where sec.SecCodigo == DP.SecCodigo select sec.SecDescripcion).First().ToString()
                                                    });

            if (results == null || results.Count() == 0)
            {
                return NoContent();
            }

            return await results.ToListAsync();
        }

        // GET: Eliminaciones/DatosPlanillas/All
        /// <summary>
        /// Obtiene todos los datos de las planillas
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<DatosPlanillaDTO>>> GetAllDatosPlanilla()
        {
            IQueryable<DatosPlanillaDTO> results = (from DP in _context.DatosPlanilla
                                                    select new DatosPlanillaDTO
                                                    {
                                                        EmpCodigo = DP.EmpCodigo,
                                                        EmpDescripcion = (from emp in _context.Empresas where emp.EmpCodigo == DP.EmpCodigo select emp.EmpDescripcion).First().ToString(),
                                                        Periodo = DP.Periodo,
                                                        RubCodigo = DP.RubCodigo,
                                                        RubDescripcion = (from rub in _context.Rubros where rub.RubCodigo == DP.RubCodigo select rub.RubDescripcion).First().ToString(),
                                                        Concepto = DP.Concepto,
                                                        EmpCodigoContraparte = DP.EmpCodigoContraparte,
                                                        EmpDescripcionContraparte = (from empC in _context.Empresas where empC.EmpCodigo == DP.EmpCodigoContraparte select empC.EmpIntercompany).First().ToString(),
                                                        MonCodigo = DP.MonCodigo,
                                                        MonDescripcion = (from mon in _context.Moneda where mon.MonCodigo == DP.MonCodigo select mon.MonDescripcion).First().ToString(),
                                                        Saldo = DP.Saldo,
                                                        SaldoPromedio = DP.SaldoPromedio,
                                                        Ind = DP.Ind,
                                                        Pond = DP.Pond,
                                                        Exposicion = DP.Exposicion,
                                                        SecCodigo = DP.SecCodigo,
                                                        SecDescripcion = (from sec in _context.Sectores where sec.SecCodigo == DP.SecCodigo select sec.SecDescripcion).First().ToString()
                                                    });

            if (results == null || results.Count() == 0)
            {
                return NoContent();
            }

            return await results.ToListAsync();
        }

        //// PUT: Eliminaciones/DatosPlanillas/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDatosPlanilla(int id, DatosPlanilla datosPlanilla)
        //{
        //    if (id != datosPlanilla.EmpCodigo)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(datosPlanilla).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DatosPlanillaExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: Eliminaciones/DatosPlanillas
        //[HttpPost]
        //public async Task<ActionResult<DatosPlanilla>> PostDatosPlanilla(DatosPlanilla datosPlanilla)
        //{
        //    _context.DatosPlanilla.Add(datosPlanilla);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (DatosPlanillaExists(datosPlanilla.EmpCodigo))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetDatosPlanilla", new { id = datosPlanilla.EmpCodigo }, datosPlanilla);
        //}

        //// DELETE: Eliminaciones/DatosPlanillas/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<DatosPlanilla>> DeleteDatosPlanilla(int id)
        //{
        //    var datosPlanilla = await _context.DatosPlanilla.FindAsync(id);
        //    if (datosPlanilla == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.DatosPlanilla.Remove(datosPlanilla);
        //    await _context.SaveChangesAsync();

        //    return datosPlanilla;
        //}

        //private bool DatosPlanillaExists(int id)
        //{
        //    return _context.DatosPlanilla.Any(e => e.EmpCodigo == id);
        //}
    }
}
