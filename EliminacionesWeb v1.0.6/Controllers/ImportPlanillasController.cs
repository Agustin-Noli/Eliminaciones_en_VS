using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EliminacionesWeb.Models;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using EliminacionesWeb.ModelsDTO;

namespace EliminacionesWeb.Controllers
{
    //[System.Web.Http.Cors.EnableCors(origins: "http://dco16liis51:9082", headers: "*", methods: "GET,POST,OPTIONS,DELETE,PUT")]
    [Route("Eliminaciones/[controller]")]
    [ApiController]
    public class ImportPlanillasController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;
        public List<ImportPlanillas> ListImpPlanillas;

        public ImportPlanillasController(EliminacionesContext_Custom context)
        {
            _context = context;
        }

        // GET: Eliminaciones/ImportPlanillas
        /// <summary>
        /// Obtiene todas las planillas importadas 
        /// </summary>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("{Sec_Codigo}")]
        public async Task<ActionResult<IEnumerable<ImportPlanillasDTO>>> GetImportPlanillas(int Sec_Codigo)
        {

            IQueryable<ImportPlanillasDTO> results = (from IP in _context.ImportPlanillas
                                                      where IP.SecCodigo == Sec_Codigo
                                                      select new ImportPlanillasDTO
                                                      {
                                                          EmpCodigo = IP.EmpCodigo,
                                                          EmpDescripcion = (from emp in _context.Empresas where emp.EmpCodigo == IP.EmpCodigo select emp.EmpDescripcion).First().ToString(),
                                                          Periodo = IP.Periodo,
                                                          NombrePlanilla = IP.NombrePlanilla,
                                                          //GrupoDescripcion = (from gru in _context.Grupos where gru.GrupoNombre == IP.Grupo select gru.GrupoNombre).First().ToString(),
                                                          Porcentaje = IP.Porcentaje,
                                                          SecCodigo = IP.SecCodigo,
                                                          MonCodigo = IP.MonCodigo,
                                                          Importacion = IP.Importacion
                                                      });

            if (results == null || results.Count() == 0)
            {
                return NoContent();
            }

            return await results.ToListAsync();
        }

        // GET: Eliminaciones/ImportPlanillas/5
        /// <summary>
        /// Obtiene las planillas filtradas por empresas
        /// </summary>
        /// <param name="Emp_Codigo"></param>
        /// <param name="Sec_COdigo"></param>
        /// <returns></returns>
        [HttpGet("ByEmpresa")]
        public async Task<ActionResult<ImportPlanillas>> GetImportPlanillasByEmpresa([FromQuery] int Emp_Codigo, [FromQuery] int Sec_Codigo)
        {
            //var importPlanillas = await _context.ImportPlanillas.Where(x => x.EmpCodigo == EMP_CODIGO).ToListAsync();
            IQueryable<ImportPlanillasDTO> results = (from IP in _context.ImportPlanillas
                                                      where IP.EmpCodigo == Emp_Codigo && IP.SecCodigo == Sec_Codigo
                                                      select new ImportPlanillasDTO
                                                      {
                                                          EmpCodigo = IP.EmpCodigo,
                                                          EmpDescripcion = (from emp in _context.Empresas where emp.EmpCodigo == IP.EmpCodigo select emp.EmpDescripcion).First().ToString(),
                                                          Periodo = IP.Periodo,
                                                          NombrePlanilla = IP.NombrePlanilla,
                                                          //GrupoDescripcion = (from gru in _context.Grupos where gru.GrupoNombre == IP.Grupo select gru.GrupoNombre).First().ToString(),
                                                          Porcentaje = IP.Porcentaje,
                                                          SecCodigo = IP.SecCodigo,
                                                          MonCodigo = IP.MonCodigo,
                                                          Importacion = IP.Importacion
                                                      });

            if (results == null || results.Count() == 0)
            {
                return NoContent();
            }

            return Ok(await results.ToListAsync());
        }

        // GET: Eliminaciones/ImportPlanillas/5
        /// <summary>
        /// Obtiene las planillas filtradas por periodo
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="Sec_COdigo"></param>
        /// <returns></returns>
        [HttpGet("ByPeriodo")]
        public async Task<ActionResult<ImportPlanillas>> GetImportPlanillasByPeriodo([FromQuery] string Periodo, [FromQuery] int Sec_Codigo)
        {
            //var importPlanillas = await _context.ImportPlanillas.FindAsync(PERIODO);
            //var importPlanillas = await _context.ImportPlanillas.Where(x => x.Periodo == PERIODO).ToListAsync();
            IQueryable<ImportPlanillasDTO> results = (from IP in _context.ImportPlanillas
                                                      where IP.Periodo == Periodo && IP.SecCodigo == Sec_Codigo
                                                      select new ImportPlanillasDTO
                                                      {
                                                          EmpCodigo = IP.EmpCodigo,
                                                          EmpDescripcion = (from emp in _context.Empresas where emp.EmpCodigo == IP.EmpCodigo select emp.EmpDescripcion).First().ToString(),
                                                          Periodo = IP.Periodo,
                                                          NombrePlanilla = IP.NombrePlanilla,
                                                          //GrupoDescripcion = (from gru in _context.Grupos where gru.GrupoNombre == IP.Grupo select gru.GrupoNombre).First().ToString(),
                                                          Porcentaje = IP.Porcentaje,
                                                          SecCodigo = IP.SecCodigo,
                                                          MonCodigo = IP.MonCodigo,
                                                          Importacion = IP.Importacion
                                                      });

            if (results == null || results.Count() == 0)
            {
                return NoContent();
            }

            return Ok(await results.ToListAsync());
        }

        //GET: Eliminaciones/ImportPlanillas/GetPlanillasCargadas
        /// <summary>
        /// Obtiene las planillas filtradas por periodo
        /// </summary>
        /// <param name = "Periodo" ></ param >
        /// < param name= "GrupoId"></param>
        /// <param name = "Sec_Codigo" ></ param >
        /// < returns ></ returns >
        [HttpGet("GetPlanillasCargadas")]
        public async Task<ActionResult<PlanillasCargadasDTO>> GetPlanillasCargadas([FromQuery] string Periodo, [FromQuery] int GrupoId, [FromQuery] int Sec_Codigo)
        {
            ListImpPlanillas = (from imp in _context.ImportPlanillas
                                where imp.Periodo == Periodo && imp.SecCodigo == Sec_Codigo /*&& imp.Importacion == "S"*/
                                select imp).ToList();

            //IQueryable<Empresas> ListEmpresasbyGrupo = (from emp in _context.Empresas
            //                                            join gemp in _context.GrupoEmpresas
            //                                                on new { emp.EmpCodigo, emp.SecCodigo } equals new { gemp.EmpCodigo, gemp.SecCodigo }
            //                                            where gemp.GrupoId == GrupoId && gemp.SecCodigo == Sec_Codigo
            //                                            select emp);

            List<PlanillasCargadasDTO> results = (from emp in _context.Empresas
                                                        join gemp in _context.GrupoEmpresas
                                                            on new { emp.EmpCodigo, emp.SecCodigo } equals new { gemp.EmpCodigo, gemp.SecCodigo }
                                                        where gemp.GrupoId == GrupoId && gemp.SecCodigo == Sec_Codigo
                                                        select new PlanillasCargadasDTO
                                                        {
                                                            EmpCodigo = emp.EmpCodigo,
                                                            EmpDescripcion = emp.EmpDescripcion,
                                                            Periodo = Periodo,
                                                            Porcentaje = emp.EmpPorcentaje,
                                                            //MonCodigo = ListImpPlanillas.Any(x => x.EmpCodigo == emp.EmpCodigo) ? (from imp in ListImpPlanillas where imp.EmpCodigo == emp.EmpCodigo select imp.MonCodigo).First() : null,
                                                            //MonDescripcion = ??,
                                                            NombrePlanilla = "",
                                                            SecCodigo = Sec_Codigo,
                                                            Importacion = "N"
                                                        }).ToList();

            foreach (var item in results)
            {
                if (ListImpPlanillas.Any(x => x.EmpCodigo == item.EmpCodigo))
                {
                    item.NombrePlanilla = (from imp in ListImpPlanillas where imp.EmpCodigo == item.EmpCodigo select imp.NombrePlanilla).FirstOrDefault();
                    item.Importacion = (from imp in ListImpPlanillas where imp.EmpCodigo == item.EmpCodigo select imp.Importacion).FirstOrDefault();
                }

            }

            if (results == null || results.Count() == 0)
            {
                return NoContent();
            }

            return Ok(results);
        }



        private bool ImportPlanillasExists(int id)
        {
            //return _context.ImportPlanillas.Any(e => e.EmpCodigo == id);
            if (ListImpPlanillas != null)
                return ListImpPlanillas.Contains(new ImportPlanillas { EmpCodigo = id});
            else
                return false;
        }


    }
}