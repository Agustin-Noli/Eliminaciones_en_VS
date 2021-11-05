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
    //[System.Web.Http.Cors.EnableCors(origins: "http://dco16liis51:9082", headers: "*", methods: "GET,POST,OPTIONS,DELETE,PUT")]
    [Route("Eliminaciones/[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;

        public GruposController(EliminacionesContext_Custom context)
        {
            _context = context;
        }

        // GET: Eliminaciones/Grupos/1
        /// <summary>
        /// Obtiene el listado completo de todos los grupos de empresas
        /// </summary>
        /// <returns></returns>
        [HttpGet("{Sec_Codigo}")]
        public async Task<ActionResult<IEnumerable<Grupos>>> GetGrupos(int Sec_Codigo)
        {
            return await _context.Grupos.Where(id => id.SecCodigo == Sec_Codigo && id.Activo == "S").ToListAsync();
        }

        // GET: Eliminaciones/Grupos/2
        /// <summary>
        /// Obtiene la empresa indicada por parametro
        /// </summary>
        /// <param name="Grupo_Id"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("GetGrupo")]
        public async Task<ActionResult<Grupos>> GetGrupo([FromQuery] int Grupo_Id, [FromQuery] int Sec_Codigo)
        {
            //var grupos = await _context.Grupos.FindAsync(Grupo_Id);
            var grupos = await _context.Grupos.Where(gru => gru.GrupoId == Grupo_Id && gru.SecCodigo == Sec_Codigo && gru.Activo == "S").SingleOrDefaultAsync();

            if (grupos == null)
            {
                return NoContent();
            }

            return grupos;
        }

        // PUT: Eliminaciones/Grupos/2
        /// <summary>
        /// Actualiza el grupo indicado por parametro
        /// </summary>
        /// <param name="Grupo_Id"></param>
        /// <param name="grupos"></param>
        /// <returns></returns>
        [HttpPut("{Grupo_Id}")]
        public async Task<IActionResult> UpdateGrupos([FromRoute] int Grupo_Id, [FromBody] Grupos grupos)
        {
            if (Grupo_Id != grupos.GrupoId)
            {
                return BadRequest();
            }

            grupos.Activo = "S";
            _context.Entry(grupos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();


                Historial log = new Historial
                {
                    UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                    Periodo = string.Empty,
                    FechaHora = DateTime.Now,
                    Accion = "Modificacion de Grupo",
                    Mensaje = "Se modifico el grupo " + grupos.GrupoNombre,
                    SecCodigo = grupos.SecCodigo
                };
                _context.Historial.Add(log);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GruposExists(Grupo_Id, grupos.SecCodigo))
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

        // POST: Eliminaciones/Grupos
        /// <summary>
        /// Create nuevo Grupo
        /// </summary>
        /// <param name="grupos"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Grupos>> CreateGrupos([FromBody] Grupos grupos)
        {
            grupos.Activo = "S";
            _context.Grupos.Add(grupos);
            await _context.SaveChangesAsync();

            Historial log = new Historial
            {
                UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                Periodo = string.Empty,
                FechaHora = DateTime.Now,
                Accion = "Alta de Grupo",
                Mensaje = "Se creo el grupo  " + grupos.GrupoNombre,
                SecCodigo = grupos.SecCodigo
            };
            _context.Historial.Add(log);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetGrupos", new { id = grupos.GrupoId }, grupos);
            return grupos;
        }   

        // DELETE: Eliminaciones/Grupos/3
        /// <summary>
        /// Elimina el grupo indicado por parametro
        /// </summary>
        /// <param name="Grupo_Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<Grupos>> DeleteGrupos([FromQuery] int Grupo_Id, [FromQuery] int Sec_Codigo)
        {
            //var grupos = await _context.Grupos.FindAsync(Grupo_Id);
            var grupos = await _context.Grupos.Where(g => g.GrupoId == Grupo_Id && g.SecCodigo == Sec_Codigo && g.Activo == "S").SingleOrDefaultAsync();

            if (grupos == null)
            {
                return NoContent();
            }

            //_context.Grupos.Remove(grupos);
            grupos.Activo = "N";
            _context.Entry(grupos).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            Historial log = new Historial
            {
                UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                Periodo = string.Empty,
                FechaHora = DateTime.Now,
                Accion = "Eliminacion de Grupo",
                Mensaje = "Se elimino el grupo " + grupos.GrupoNombre,
                SecCodigo = grupos.SecCodigo
            };
            _context.Historial.Add(log);
            await _context.SaveChangesAsync();

            return grupos;
        }

        private bool GruposExists(int Grupo_Id, int Sec_Codigo)
        {
            return _context.Grupos.Any(e => e.GrupoId == Grupo_Id && e.SecCodigo == Sec_Codigo);
        }
    }
}
