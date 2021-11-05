using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EliminacionesWeb.Models;
using EliminacionesWeb.ModelsDTO;
using EliminacionesWeb.Helpers;

namespace EliminacionesWeb.Controllers
{
    //[System.Web.Http.Cors.EnableCors(origins: "http://dco16liis51:9082", headers: "*", methods: "GET,POST,OPTIONS,DELETE,PUT")]
    [Route("Eliminaciones/[controller]")]
    [ApiController]
    public class AjustesController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;

        public AjustesController(EliminacionesContext_Custom context)
        {
            _context = context;
        }

        // GET: Eliminaciones/Ajustes/1
        /// <summary>
        /// Obtiene todos los Ajustes
        /// </summary>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("{Sec_Codigo}")]
        public async Task<ActionResult<IEnumerable<AjustesDTO>>> GetAjustes(int Sec_Codigo)
        {
            IQueryable<AjustesDTO> results = (from _ajustes in _context.Ajustes
                                              where _ajustes.SecCodigo == Sec_Codigo
                                              select new AjustesDTO
                                              {
                                                  EmpCodigo = _ajustes.EmpCodigo,
                                                  EmpDescripcion = (from emp in _context.Empresas where emp.EmpCodigo == _ajustes.EmpCodigo select emp.EmpDescripcion).First().ToString(),
                                                  Periodo = _ajustes.Periodo,
                                                  RubCodigo = _ajustes.RubCodigo,
                                                  RubDescripcion = (from rub in _context.Rubros where rub.RubCodigo == _ajustes.RubCodigo select rub.RubDescripcion).First().ToString(),
                                                  EmpCodigoContraparte = _ajustes.EmpCodigoContraparte,
                                                  EmpDescripcionContraparte = (from empC in _context.Empresas where empC.EmpCodigo == _ajustes.EmpCodigoContraparte select empC.EmpIntercompany).First().ToString(),
                                                  AjuSaldo = _ajustes.AjuSaldo,
                                                  AjuSaldoPromedio = _ajustes.AjuSaldoPromedio,
                                                  SecCodigo = _ajustes.SecCodigo,
                                                  SecDescripcion = (from sec in _context.Sectores where sec.SecCodigo == _ajustes.SecCodigo select sec.SecDescripcion).First().ToString()
                                              });

            if (results == null || results.Count() == 0)
            {
                return NoContent();
            }

            return await results.ToListAsync();
        }

        // GET: Eliminaciones/Ajustes/GetAjustesByPeriodo?Periodo=072020&Sec_Codigo=1
        /// <summary>
        /// Obtiene los ajustes por periodo
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("GetAjustesByPeriodo")]
        public async Task<ActionResult<IEnumerable<AjustesDTO>>> GetAjustesByPeriodo([FromQuery] string Periodo, [FromQuery] int Sec_Codigo)
        {
            //var ajustes = await _context.Ajustes.FindAsync(Periodo);
            //var ajustes = await _context.Ajustes.Where(x => x.Periodo == Periodo).ToListAsync();

            IQueryable<AjustesDTO> results = (from _ajustes in _context.Ajustes
                                              where _ajustes.Periodo == Periodo && _ajustes.SecCodigo == Sec_Codigo
                                              select new AjustesDTO
                                              {
                                                  EmpCodigo = _ajustes.EmpCodigo,
                                                  EmpDescripcion = (from emp in _context.Empresas where emp.EmpCodigo == _ajustes.EmpCodigo select emp.EmpDescripcion).First().ToString(),
                                                  Periodo = _ajustes.Periodo,
                                                  RubCodigo = _ajustes.RubCodigo,
                                                  RubDescripcion = (from rub in _context.Rubros where rub.RubCodigo == _ajustes.RubCodigo select rub.RubDescripcion).First().ToString(),
                                                  EmpCodigoContraparte = _ajustes.EmpCodigoContraparte,
                                                  EmpDescripcionContraparte = (from empC in _context.Empresas where empC.EmpCodigo == _ajustes.EmpCodigoContraparte select empC.EmpIntercompany).First().ToString(),
                                                  AjuSaldo = _ajustes.AjuSaldo,
                                                  AjuSaldoPromedio = _ajustes.AjuSaldoPromedio,
                                                  SecCodigo = _ajustes.SecCodigo,
                                                  SecDescripcion = (from sec in _context.Sectores where sec.SecCodigo == _ajustes.SecCodigo select sec.SecDescripcion).First().ToString()
                                              });

            if (results == null || results.Count() == 0)
            {
                return NoContent();
            }

            return await results.ToListAsync();
        }

        // PUT: Eliminaciones/Ajustes/5
        /// <summary>
        /// Actualiza un Ajuste
        /// </summary>
        /// <param name="ajustes"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateAjustes(Ajustes ajuste)
        {
            if (ajuste.EmpCodigo == 0 || ajuste.Periodo == null || ajuste.RubCodigo == null || ajuste.EmpCodigoContraparte == 0 || ajuste.SecCodigo == 0)
            {
                return BadRequest();
            }

            _context.Entry(ajuste).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                Historial log = new Historial
                {
                    UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                    Periodo = string.Empty,
                    FechaHora = DateTime.Now,
                    Accion = "Modificacion de Ajuste",
                    Mensaje = "Se Modifico un Ajuste del periodo " + ajuste.Periodo,
                    SecCodigo = ajuste.SecCodigo
                };
                _context.Historial.Add(log);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AjustesExists(ajuste))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: Eliminaciones/Ajustes
        /// <summary>
        /// Inserta un nuevo Ajuste
        /// </summary>
        /// <param name="ajuste"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Ajustes>> InsertAjustes([FromBody] Ajustes ajuste)
        {
            _context.Ajustes.Add(ajuste);
            try
            {
                await _context.SaveChangesAsync();

                Historial log = new Historial
                {
                    UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                    Periodo = string.Empty,
                    FechaHora = DateTime.Now,
                    Accion = "Alta de Ajuste",
                    Mensaje = "Se creo un Ajuste para el periodo " + ajuste.Periodo,
                    SecCodigo = ajuste.SecCodigo
                };
                _context.Historial.Add(log);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                if (AjustesExists(ajuste))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //return CreatedAtAction("GetAjustes", new { id = ajuste.EmpCodigo }, ajuste);
            return ajuste;
        }

        // DELETE: Eliminaciones/Ajustes/5
        /// <summary>
        /// Elimina el ajuste indicado por parametro
        /// </summary>
        /// <param name="ajuste"></param>
        /// <returns></returns>
        [HttpPost("DeleteAjustes")]
        public async Task<ActionResult<Ajustes>> DeleteAjustes(Ajustes ajuste)
        {
            if (ajuste.EmpCodigo == 0 || ajuste.Periodo == null || ajuste.RubCodigo == null || ajuste.EmpCodigoContraparte == 0 || ajuste.SecCodigo == 0)
                return BadRequest();

            var ajustes = await _context.Ajustes.Where(a => a.EmpCodigo == ajuste.EmpCodigo && a.Periodo == ajuste.Periodo && a.RubCodigo == ajuste.RubCodigo &&
                                                        a.EmpCodigoContraparte == ajuste.EmpCodigoContraparte && a.AjuSaldo == ajuste.AjuSaldo &&
                                                        a.AjuSaldoPromedio == ajuste.AjuSaldoPromedio && a.SecCodigo == ajuste.SecCodigo).SingleOrDefaultAsync();


            if (ajustes == null)
            {
                return NoContent();
            }

            _context.Ajustes.Remove(ajustes);
            await _context.SaveChangesAsync();

            Historial log = new Historial
            {
                UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                Periodo = string.Empty,
                FechaHora = DateTime.Now,
                Accion = "Eliminacion de Ajuste",
                Mensaje = "Se elimino un Ajuste del periodo " + ajuste.Periodo,
                SecCodigo = ajuste.SecCodigo
            };
            _context.Historial.Add(log);
            await _context.SaveChangesAsync();

            return ajustes;
        }

        private bool AjustesExists(Ajustes ajustes)
        {
            return _context.Ajustes.Any(e => e.EmpCodigo == ajustes.EmpCodigo && e.Periodo == ajustes.Periodo && e.RubCodigo == ajustes.RubCodigo && e.EmpCodigoContraparte == ajustes.EmpCodigoContraparte && e.SecCodigo == ajustes.SecCodigo);
        }
    }
}
