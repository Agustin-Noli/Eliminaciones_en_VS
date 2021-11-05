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
    //[System.Web.Http.Cors.EnableCors(origins: "http://dco16liis51:9082", headers: "*", methods: "GET,POST,OPTIONS,DELETE,PUT")]
    [Route("Eliminaciones/[controller]")]
    [ApiController]
    public class MonedasController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;

        public MonedasController(EliminacionesContext_Custom context)
        {
            _context = context;
        }

        // GET: Eliminaciones/Monedas
        /// <summary>
        /// Obtiene todas las monedas cargadas en la base
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moneda>>> GetMoneda()
        {
            return await _context.Moneda.ToListAsync();
        }

        // GET: Eliminaciones/Monedas/5
        /// <summary>
        /// Obtiene la moneda indicada por parametro
        /// </summary>
        /// <param name="MON_CODIGO"></param>
        /// <returns></returns>
        [HttpGet("{MON_CODIGO}")]
        public async Task<ActionResult<Moneda>> GetMoneda(int MON_CODIGO)
        {
            var moneda = await _context.Moneda.FindAsync(MON_CODIGO);

            if (moneda == null)
            {
                return NoContent();
            }

            return moneda;
        }

        //// PUT: Eliminaciones/Monedas/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMoneda(int id, Moneda moneda)
        //{
        //    if (id != moneda.MonCodigo)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(moneda).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MonedaExists(id))
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

        //// POST: Eliminaciones/Monedas
        //[HttpPost]
        //public async Task<ActionResult<Moneda>> PostMoneda(Moneda moneda)
        //{
        //    _context.Moneda.Add(moneda);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (MonedaExists(moneda.MonCodigo))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetMoneda", new { id = moneda.MonCodigo }, moneda);
        //}

        //// DELETE: Eliminaciones/Monedas/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Moneda>> DeleteMoneda(int id)
        //{
        //    var moneda = await _context.Moneda.FindAsync(id);
        //    if (moneda == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Moneda.Remove(moneda);
        //    await _context.SaveChangesAsync();

        //    return moneda;
        //}

        private bool MonedaExists(int id)
        {
            return _context.Moneda.Any(e => e.MonCodigo == id);
        }
    }
}
