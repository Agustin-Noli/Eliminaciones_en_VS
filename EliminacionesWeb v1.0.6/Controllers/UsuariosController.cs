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
    public class UsuariosController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;

        public UsuariosController(EliminacionesContext_Custom context)
        {
            _context = context;
        }

        // GET: Eliminaciones/Usuarios
        /// <summary>
        /// Obtiene todos los usuarios cargados en base
        /// </summary>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("{Sec_Codigo}")]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios(int Sec_Codigo)
        {
            //return await _context.Usuarios.Where(usu => usu.SecCodigo == Sec_Codigo && usu.Activo == "S").ToListAsync();
            IQueryable<UsuarioDTO> results = (from _usu in _context.Usuarios
                                              where _usu.SecCodigo == Sec_Codigo && _usu.Activo == "S"
                                              select new UsuarioDTO { 
                                                  UsuLegajo = _usu.UsuLegajo,
                                                  UsuNombre = _usu.UsuNombre,
                                                   PerCodigo = _usu.PerCodigo,
                                                   UsuPerfil = (from per in _context.Perfiles where per.PerCodigo == _usu.PerCodigo select per.PerDescripcion).First().ToString(),
                                                   UsuFecalta = _usu.UsuFecalta,
                                                   SecCodigo = _usu.SecCodigo,
                                                   Activo = _usu.Activo
                                              });

            if (results == null)
                return NoContent();

            return await results.ToListAsync();
        }

        // GET: Eliminaciones/Usuarios/5
        /// <summary>
        /// Obtiene el usuario solicitado por parametro
        /// </summary>
        /// <param name="Usu_Legajo"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("GetUsuario")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario([FromQuery] string Usu_Legajo, [FromQuery] int Sec_Codigo)
        {
            //var usuarios = await _context.Usuarios.FindAsync(Usu_Legajo);
            //var usuarios = await _context.Usuarios.Where(usu => usu.UsuLegajo == Usu_Legajo && usu.SecCodigo == Sec_Codigo && usu.Activo == "S").SingleOrDefaultAsync();

            //if (usuarios == null)
            //{
            //    return NoContent();
            //}

            //return usuarios;

            var results = (from _usu in _context.Usuarios
                                              where _usu.UsuLegajo == Usu_Legajo && _usu.SecCodigo == Sec_Codigo && _usu.Activo == "S"
                                              select new UsuarioDTO
                                              {
                                                  UsuLegajo = _usu.UsuLegajo,
                                                  UsuNombre = _usu.UsuNombre,
                                                  PerCodigo = _usu.PerCodigo,
                                                  UsuPerfil = (from per in _context.Perfiles where per.PerCodigo == _usu.PerCodigo select per.PerDescripcion).First().ToString(),
                                                  UsuFecalta = _usu.UsuFecalta,
                                                  SecCodigo = _usu.SecCodigo,
                                                  Activo = _usu.Activo
                                              }).SingleOrDefaultAsync();

            if (results == null)
                return NoContent();

            return await results;
        }

        // PUT: Eliminaciones/Usuarios/5
        /// <summary>
        /// Actualiza los datos de usuario definido por parametro
        /// </summary>
        /// <param name="Usu_Legajo"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPut("{Usu_Legajo}")]
        public async Task<IActionResult> UpdateUsuarios(string Usu_Legajo, Usuarios usuario)
        {
            if (Usu_Legajo != usuario.UsuLegajo)
            {
                return BadRequest();
            }

            usuario.Activo = "S";
            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                Historial log = new Historial
                {
                    UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                    Periodo = string.Empty,
                    FechaHora = DateTime.Now,
                    Accion = "Modificacion de usuario",
                    Mensaje = "Se modifico el usuario " + usuario.UsuLegajo,
                    SecCodigo = usuario.SecCodigo
                };
                _context.Historial.Add(log);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExists(usuario))
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

        // POST: Eliminaciones/Usuarios
        /// <summary>
        /// Da de alta un nuevo usuaario en la base de eliminaciones
        /// </summary>
        /// <param name="usuarios"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Usuarios>> CreateUsuarios([FromBody] Usuarios usuario)
        {
            usuario.Activo = "S";
            _context.Usuarios.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();

                Historial log = new Historial
                {
                    UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                    Periodo = string.Empty,
                    FechaHora = DateTime.Now,
                    Accion = "Alta de usuario",
                    Mensaje = "Se creo el usuario " + usuario.UsuLegajo,
                    SecCodigo = usuario.SecCodigo
                };
                _context.Historial.Add(log);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                if (UsuariosExists(usuario))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //return CreatedAtAction("GetUsuarios", new { id = usuario.UsuLegajo }, usuario);
            return usuario;
        }

        // DELETE: Eliminaciones/Usuarios/5
        /// <summary>
        /// Elimina el usuario indicado por parametro
        /// </summary>
        /// <param name="Usu_Legajo"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<Usuarios>> DeleteUsuarios([FromQuery] string Usu_Legajo, [FromQuery] int Sec_Codigo)
        {
            var usuarios = await _context.Usuarios.Where(usu => usu.UsuLegajo == Usu_Legajo && usu.SecCodigo == Sec_Codigo && usu.Activo == "S").SingleOrDefaultAsync();
            if (usuarios == null)
            {
                return NoContent();
            }

            //_context.Usuarios.Remove(usuarios);
            usuarios.Activo = "N";
            _context.Entry(usuarios).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            Historial log = new Historial
            {
                UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                Periodo = string.Empty,
                FechaHora = DateTime.Now,
                Accion = "Eliminacion de usuario",
                Mensaje = "Se elimino el usuario " + Usu_Legajo,
                SecCodigo = Sec_Codigo
            };
            _context.Historial.Add(log);
            await _context.SaveChangesAsync();

            return usuarios;
        }

        // GET: Eliminaciones/Usuarios/GetPerfiles
        /// <summary>
        /// Obtiene el listado completo de todos los perfiles de la aplicacion
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPerfiles")]
        public async Task<ActionResult<IEnumerable<Perfiles>>> GetPerfiles()
        {
            return await _context.Perfiles.ToListAsync();
        }


        private bool UsuariosExists(Usuarios usuario)
        {
            return _context.Usuarios.Any(e => e.UsuLegajo == usuario.UsuLegajo && e.SecCodigo == usuario.SecCodigo);
        }
    }
}
