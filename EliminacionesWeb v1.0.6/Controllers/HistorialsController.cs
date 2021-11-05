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
    [Route("Eliminaciones/[controller]")]
    [ApiController]
    public class HistorialsController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;

        public HistorialsController(EliminacionesContext_Custom context)
        {
            _context = context;
        }

        // GET: Eliminaciones/Historials
        /// <summary>
        /// Obtiene todos los Ajustes
        /// </summary>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("{Sec_Codigo}")]
        public async Task<ActionResult<IEnumerable<HistorialDTO>>> GetHistorial(int Sec_Codigo)
        {
            IQueryable<HistorialDTO> results = (from _hist in _context.Historial
                                                where _hist.SecCodigo == Sec_Codigo
                                                select new HistorialDTO
                                                {
                                                    HistId = _hist.HistId,
                                                    UsuLegajo = _hist.UsuLegajo,
                                                    UsuNombre = (from usu in _context.Usuarios where usu.UsuLegajo == _hist.UsuLegajo select usu.UsuNombre).First().ToString(),
                                                    Periodo = _hist.Periodo,
                                                    FechaHora = _hist.FechaHora,
                                                    Accion = _hist.Accion,
                                                    Mensaje = _hist.Mensaje,
                                                    SecCodigo = _hist.SecCodigo,
                                                    SecDescripcion = (from sec in _context.Sectores where sec.SecCodigo == _hist.SecCodigo select sec.SecDescripcion).First().ToString()
                                                }) ;

            if (results == null || results.Count() == 0)
            {
                return NoContent();
            }

            return await results.ToListAsync();
        }

        // POST: Eliminaciones/Historials
        //[HttpPost]
        //public async Task<ActionResult<Historial>> PostHistorial(Historial historial)
        //{
        //    _context.Historial.Add(historial);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetHistorial", new { id = historial.HistId }, historial);
        //}
      

        //private bool HistorialExists(int id)
        //{
        //    return _context.Historial.Any(e => e.HistId == id);
        //}
    }
}
