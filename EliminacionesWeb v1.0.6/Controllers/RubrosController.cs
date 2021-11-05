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
    [System.Web.Http.Cors.EnableCors(origins: "http://dco16liis51:9082", headers: "*", methods: "GET,POST,OPTIONS,DELETE,PUT")]
    [Route("Eliminaciones/[controller]")]
    [ApiController]
    public class RubrosController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;

        public RubrosController(EliminacionesContext_Custom context)
        {
            _context = context;
        }

        // GET: Eliminaciones/Rubros
        /// <summary>
        /// Trae todos los rubros cargados en base
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rubros>>> GetRubros()
        {
            return await _context.Rubros.ToListAsync();
        }

        // GET: Eliminaciones/Rubros/5
        /// <summary>
        /// Obtiene el Rubro indicado por parametro
        /// </summary>
        /// <param name="RUB_CODIGO"></param>
        /// <returns></returns>
        [HttpGet("{RUB_CODIGO}")]
        public async Task<ActionResult<Rubros>> GetRubros(string RUB_CODIGO)
        {
            var rubros = await _context.Rubros.FindAsync(RUB_CODIGO);

            if (rubros == null)
            {
                return NoContent();
            }

            return rubros;
        }

        private bool RubrosExists(string id)
        {
            return _context.Rubros.Any(e => e.RubCodigo == id);
        }


        // GET: /Eliminaciones/rubros/GetRubrosCabecera
        /// <summary>
        /// Trae todos los rubros cabeceras
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRubrosCabecera")]
        public async Task<ActionResult<IEnumerable<RubrosCabecera>>> GetRubrosCabecera()
        {
            return await _context.RubrosCabecera.ToListAsync();
        }

        // GET: /Eliminaciones/rubros/GetGrandesRubros
        /// <summary>
        /// Trae todos los grandes rubros
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetGrandesRubros")]
        public async Task<ActionResult<IEnumerable<GrandesRubros>>> GetGrandesRubros()
        {
            return await _context.GrandesRubros.ToListAsync();
        }

        // GET: /Eliminaciones/rubros/GetSubRubros
        /// <summary>
        /// Trae los Subrubros cargados en base
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSubRubros")]
        public async Task<ActionResult<IEnumerable<SubRubros>>> GetSubRubros()
        {
            return await _context.SubRubros.Where(sr => sr.Activo == "S").ToListAsync();
        }


        // GET: /Eliminaciones/rubros/GetRelacionesRubros
        /// <summary>
        /// Trae todos las relaciones de los rubros cargados en base
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRelacionesRubros")]
        public async Task<ActionResult<IEnumerable<RelacionRubrosDTO>>> GetRelacionesRubros()
        {
            IQueryable<RelacionRubrosDTO> results = (from _rel in _context.RelacionRubros
                                                     where _rel.Activo == "S"
                                                     select new RelacionRubrosDTO
                                                     {
                                                         RubcabCodigo = _rel.RubcabCodigo,
                                                         RubcabDescripcion = ((from rc in _context.RubrosCabecera where rc.RubcabCodigo == _rel.RubcabCodigo select rc.RubcabDescripcion).First().Count() > 0) ? (from rc in _context.RubrosCabecera where rc.RubcabCodigo == _rel.RubcabCodigo select rc.RubcabDescripcion.TrimEnd()).First().ToString() : null,
                                                         GranrubCodigo = _rel.GranrubCodigo,
                                                         GranrubDescripcion = ((from gr in _context.GrandesRubros where gr.GranrubCodigo == _rel.GranrubCodigo select gr.GranrubDescripcion).First().Count() > 0) ? (from gr in _context.GrandesRubros where gr.GranrubCodigo == _rel.GranrubCodigo select gr.GranrubDescripcion.TrimEnd()).First().ToString() : null,
                                                         SubrubId = _rel.SubrubId,
                                                         SubrubDescripcion = ((from sr in _context.SubRubros where sr.SubrubId == _rel.SubrubId select sr.SubrubDescripcion).First().Count() > 0) ? (from sr in _context.SubRubros where sr.SubrubId == _rel.SubrubId select sr.SubrubDescripcion.TrimEnd()).First().ToString() : null,
                                                         RubCodigo = _rel.RubCodigo,
                                                         RubroDescripcion = ((from rub in _context.Rubros where rub.RubCodigo == _rel.RubCodigo select rub.RubDescripcion).First().Count() > 0) ? (from rub in _context.Rubros where rub.RubCodigo == _rel.RubCodigo select rub.RubDescripcion.TrimEnd()).First().ToString() : null,
                                                     });

            if (results == null || results.Count() == 0)
            {
                return NoContent();
            }

            return await results.ToListAsync();
        }

        // POST: Eliminaciones/Rubros/CreateRelacionRubro
        /// <summary>
        /// Da de alta una nueva relacion de rubros
        /// </summary>
        /// <param name="RelRubros"></param>
        /// <returns></returns>
        [HttpPost("CreateRelacionRubro")]
        public async Task<ActionResult<RelacionRubros>> CreateRelacionRubro([FromBody] RelacionRubros RelRubro)
        {
            RelRubro.Activo = "S";

            _context.RelacionRubros.Add(RelRubro);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                if (RelacionExists(RelRubro))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return RelRubro;
        }

        // DELETE: Eliminaciones/Rubros/DeleteRelacionRubros
        /// <summary>
        /// Elimina la relacion de rubros enviada por parametro
        /// </summary>
        /// <param name="RelRubro"></param>
        /// <returns></returns>
        [HttpPost("DeleteRelacionRubros")]
        public async Task<ActionResult<RelacionRubros>> DeleteRelacionRubros([FromBody] RelacionRubros RelRubro)
        {
            if (RelRubro.RubcabCodigo == null || RelRubro.GranrubCodigo == null || RelRubro.SubrubId == 0 || RelRubro.RubCodigo == null)
                return BadRequest();

            var relacion = await _context.RelacionRubros.Where(r => r.RubcabCodigo == RelRubro.RubcabCodigo && r.GranrubCodigo == RelRubro.GranrubCodigo && r.SubrubId == RelRubro.SubrubId &&
                                                                r.RubCodigo == RelRubro.RubCodigo).SingleOrDefaultAsync();

            if (relacion == null)
            {
                return NoContent();
            }

            // borrado logico en la tabla relaciones Rubros
            relacion.Activo = "N";
            _context.Entry(relacion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return relacion;
        }


        private bool RelacionExists(RelacionRubros RelRubro)
        {
            return _context.RelacionRubros.Any(e => e.RubcabCodigo == RelRubro.RubcabCodigo && e.GranrubCodigo == RelRubro.GranrubCodigo && e.SubrubId == RelRubro.SubrubId && e.RubCodigo == RelRubro.RubCodigo);
        }


        // PUT: Eliminaciones/Rubros/UpdateSubRubro
        /// <summary>
        /// Actualiza el subrubro indicado por parametro
        /// </summary>
        /// <param name="Subrub_Id"></param>
        /// <param name="SubRub"></param>
        /// <returns></returns>
        [HttpPut("UpdateSubRubro")]
        public async Task<IActionResult> UpdateSubRubro([FromQuery] int Subrub_Id, [FromBody] SubRubros SubRub)
        {
            if (Subrub_Id != SubRub.SubrubId)
            {
                return BadRequest();
            }

            _context.Entry(SubRub).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubRubroExists(SubRub))
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

        // POST: Eliminaciones/Rubros/CreateSubRubro
        /// <summary>
        /// Create nuevo Grupo
        /// </summary>
        /// <param name="SubRub"></param>
        /// <returns></returns>
        [HttpPost("CreateSubRubro")]
        public async Task<ActionResult<SubRubros>> CreateSubRubro([FromBody] SubRubros SubRub)
        {
            SubRub.Activo = "S";
            _context.SubRubros.Add(SubRub);
            await _context.SaveChangesAsync();

            return SubRub;
        }

        // DELETE: Eliminaciones/Rubros/DeleteSubRubros
        /// <summary>
        /// Elimina el subRubro indicado por parametro
        /// </summary>
        /// <param name="Subrub_Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteSubRubros")]
        public async Task<ActionResult<SubRubros>> DeleteSubRubros([FromQuery] int Subrub_Id)
        {
            var SubRubro = await _context.SubRubros.Where(sr => sr.SubrubId == Subrub_Id && sr.Activo == "S").SingleOrDefaultAsync();

            if (SubRubro == null)
            {
                return NoContent();
            }

            // borrado logico en la tabla subrubros
            SubRubro.Activo = "N";
            _context.Entry(SubRubro).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Hago un borrado logico de los subrubros que estan cargados en la tabla de relaciones rubros
            List<RelacionRubros> relacionesRubros = _context.RelacionRubros.Where(rr => rr.SubrubId == SubRubro.SubrubId).ToList();
            if (relacionesRubros!= null && relacionesRubros.Count > 0)
                foreach (var rel in relacionesRubros)
                {
                    rel.Activo = "N";
                    _context.Entry(rel).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

            return SubRubro;
        }

        private bool SubRubroExists(SubRubros SubRub)
        {
            return _context.SubRubros.Any(sr => sr.SubrubId == SubRub.SubrubId);
        }

    }
}
