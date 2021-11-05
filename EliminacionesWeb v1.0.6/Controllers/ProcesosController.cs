using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EliminacionesWeb.Models;
using EliminacionesWeb.ModelsDTO;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Web.Http.Cors;
using System.Threading;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using EliminacionesWeb.Helpers;

namespace EliminacionesWeb.Controllers
{
    //[System.Web.Http.Cors.EnableCors(origins: "http://dco16liis51:9082", headers: "*", methods: "GET,POST,OPTIONS,DELETE,PUT")]
    [Route("Eliminaciones/[controller]")]
    [ApiController]
    public class ProcesosController : ControllerBase
    {
        private readonly EliminacionesContext_Custom _context;
        private readonly IConfiguration Configuration;

        public ProcesosController(EliminacionesContext_Custom context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: Eliminaciones/Procesos/GetEliminaciones
        /// <summary>
        /// Obtiene las ultimas Eliminaciones del periodo generadas por la accion calcular
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("GetEliminaciones")]
        public async Task<ActionResult<IEnumerable<EliminacionesDTO>>> GetEliminaciones([FromQuery] string Periodo, [FromQuery] int Sec_Codigo)
        {

            IQueryable<EliminacionesDTO> results = ObtenerEliminaciones(Periodo, Sec_Codigo);

            if (results == null || results.Count() == 0)
            {
                return NoContent();
            }

            return await results.ToListAsync();
        }


        // POST: https://localhost:44368/Eliminaciones/Procesos/Calcular?Periodo=072020&GrupoId=1&Sec_Codigo=1
        /// <summary>
        /// Calcula los totales entre los datos de las planillas y los ajustes y los inserta en la tabla Eliminaciones
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="GrupoId"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpPost("Calcular")]
        public async Task<ActionResult<ResponseDTO>> Calcular([FromQuery]string Periodo, [FromQuery]int GrupoId, [FromQuery]int Sec_Codigo)
        {
            try
            {
                if ((Periodo == null || Periodo == "") && Sec_Codigo == 0)
                    return BadRequest();
                ResponseDTO response = new ResponseDTO();

                int result = _context.Database.ExecuteSqlCommand("[dbo].[SP_Calcular] @Periodo={0}, @GrupoId={1}, @Sector={2}", Periodo, GrupoId, Sec_Codigo);

                if (result != 0)
                    response.mensaje = "OK";
                else
                    response.mensaje = "ERROR";

                Historial log = new Historial
                {
                    UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                    Periodo = string.Empty,
                    FechaHora = DateTime.Now,
                    Accion = "Calcular",
                    Mensaje = "Estado del calculo:  " + response.mensaje,
                    SecCodigo = Sec_Codigo
                };
                _context.Historial.Add(log);
                await _context.SaveChangesAsync();


                return response;
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        // POST: https://localhost:44368/Eliminaciones/Procesos/GenerarTxt?Periodo=072020&Sec_Codigo=1
        /// <summary>
        /// Genera el archivo txt que se envia a hyperion
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpPost("GenerarTxt")]
        public async Task<ActionResult<ResponseDTO>> GenerarTxt([FromQuery]string Periodo, [FromQuery]int Sec_Codigo)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                //string ServerName = @"C:\";
                string PathOutput = Configuration["PathOutput"]; //@"C:\Cristian Galicia\PROYECTO ELIMINACIONES\TXT_Output\";
                string NombreArchivo = Configuration["NombreArchivo"]; //"Eli_MultiEmp.txt";

                string Pathfile = PathOutput + NombreArchivo;

                List<txtOutputDTO> ContenidoTxt = _context.txtOutputDTO.FromSqlRaw("[dbo].[SP_GenerarTxt] @Periodo={0}, @Sector={1}", Periodo, Sec_Codigo).ToList();

                // si el archivo no existe lo creo
                if (!System.IO.File.Exists(Pathfile))
                {
                    using (FileStream fs = System.IO.File.Create(Pathfile))
                    {
                        fs.Close();
                    }
                }

                // piso el contenido con lo nuevo generado
                using (TextWriter tw = new StreamWriter(Pathfile))
                {
                    // inserto el titulo
                    tw.WriteLine("scenario" + "\t" + "version" + "\t" + "año" + "\t" + "periodo" + "\t" + "intercompany" + "\t" + "divisa" + "\t" + "empresa" + "\t" + "saldos" + "\t" + "rubro" + "\t" + "data");

                    // inserto las eliminaciones del periodo
                    foreach (txtOutputDTO x in ContenidoTxt)
                    {
                        tw.WriteLine(x.scenario + "\t" + x.version + "\t" + x.año + "\t" + x.periodo + "\t" + x.intercompany + "\t" + x.divisa + "\t" + x.empresa + "\t" + x.saldos + "\t" + x.rubro + "\t" + x.data);
                    }

                    tw.Close();
                }

                response.mensaje = "OK";
                return response;
            }
            catch (Exception ex)
            {
                response.mensaje = "ERROR: " + ex.Message;
                return response;
            }
            finally
            {

                Historial log = new Historial
                {
                    UsuLegajo = HttpContext.User.Identity.Name.Split("\\")[1],
                    Periodo = string.Empty,
                    FechaHora = DateTime.Now,
                    Accion = "Generacion de TXT",
                    Mensaje = "Estado del archivo txt:  " + response.mensaje,
                    SecCodigo = Sec_Codigo
                };
                _context.Historial.Add(log);
                await _context.SaveChangesAsync();
            }
        }




        // GET: Eliminaciones/Procesos/GenerarExcel
        /// <summary>
        /// Genera excel
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="Sec_Codigo"></param>
        /// <returns></returns>
        [HttpGet("GenerarExcel")]
        public ActionResult GenerarExcel(CancellationToken cancellationToken, [FromQuery]string Periodo, [FromQuery]int Sec_Codigo)
        {
            try
            {
                List<ReporteEliminacionesDTO> ListaEliminaciones = ObtenerReporteEliminaciones(Periodo, Sec_Codigo).ToList();

                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Eliminaciones");

                    //var picture = workSheet.Drawings.AddPicture("Logo", logo);
                    //picture.From.Column = 0;
                    //picture.From.Row = 0;
                    //picture.SetSize(140, 35);

                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-AR");

                    // CABECERA TITULO
                    //workSheet.Cells["A1:H1"].Merge = true;
                    //workSheet.Cells["A2:H2"].Merge = true;
                    //workSheet.Cells["A1:H1"].Style.Border.BorderAround(ExcelBorderStyle.None);
                    workSheet.Cells["A2"].Value = "Reporte - Eliminaciones";
                    workSheet.Cells["A2:G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["A2:G2"].Style.Font.Bold = true;
                    workSheet.Cells["A2:G2"].Style.Font.Size = 16;
                    workSheet.Cells["A2:G2"].Style.Font.Name = "Calibri Light";
                    //workSheet.Cells["A1:H1"].Style.Font.UnderLine = true;

                    // le doy formato al periodo para mostrar
                    DateTime MostrarPeriodo = Convert.ToDateTime("01/" + Periodo.Substring(0, 2) + "/" + Periodo.Substring(2, 4));
                    //workSheet.Cells["A3:H3"].Merge = true;
                    //workSheet.Cells["A4:H4"].Merge = true;
                    //workSheet.Cells["A5:H5"].Merge = true;
                    //workSheet.Cells["A2:H2"].Style.Border.BorderAround(ExcelBorderStyle.None);
                    workSheet.Cells["A3"].Value = "Periodo " + MostrarPeriodo.ToString("MMMM yyyy");
                    workSheet.Cells["A3:G3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["A3:G3"].Style.Font.Bold = true;
                    workSheet.Cells["A3:G3"].Style.Font.Size = 16;
                    workSheet.Cells["A3:G3"].Style.Font.Name = "Calibri Light";
                    //workSheet.Cells["A2:H2"].Style.Font.UnderLine = true;

                    // GRILLA
                    workSheet.Cells["B6"].Value = "Origen"; workSheet.Cells["B6"].Style.Font.Bold = true;
                    workSheet.Cells["C6"].Value = "Contraparte"; workSheet.Cells["C6"].Style.Font.Bold = true;
                    //workSheet.Cells["D6"].Value = "Concepto"; workSheet.Cells["D6"].Style.Font.Bold = true;
                    workSheet.Cells["D6"].Value = "BCRA"; workSheet.Cells["D6"].Style.Font.Bold = true;
                    workSheet.Cells["E6"].Value = "Moneda"; workSheet.Cells["E6"].Style.Font.Bold = true;
                    workSheet.Cells["F6"].Value = "Saldo (miles)"; workSheet.Cells["F6"].Style.Font.Bold = true;
                    workSheet.Cells["G6"].Value = "Saldo Promedio"; workSheet.Cells["G6"].Style.Font.Bold = true;
                    workSheet.Cells["A6:G6"].Style.Fill.PatternType = ExcelFillStyle.Solid; workSheet.Cells["A6:G6"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                    workSheet.Cells["B6:G6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int fila = 7; // numero de fila para comenzar a llenar 
                    decimal? totalSubRubros = 0, totalSubRubrosProm = 0, totalGrandesRubros = 0, totalGrandesRubrosProm = 0, totalCabecera = 0, totalCabeceraProm = 0, totalGeneral = 0, totalGeneralProm = 0;

                    var listRubrosCabecera = (from lista in ListaEliminaciones
                                              group lista by new { lista.RubcabCodigo, lista.RubcabDescripcion } into g
                                              select new { codigo = g.Key.RubcabCodigo, descripcion = g.Key.RubcabDescripcion });

                    var listGrandesRubros = (from lista in ListaEliminaciones
                                             group lista by new { lista.GranrubCodigo, lista.GranrubDescripcion } into g
                                             select new { codigo = g.Key.GranrubCodigo, descripcion = g.Key.GranrubDescripcion });

                    var listSubRubros = (from lista in ListaEliminaciones
                                         group lista by new { lista.SubrubId, lista.SubrubDescripcion } into g
                                         select new { codigo = g.Key.SubrubId, descripcion = g.Key.SubrubDescripcion });

                    foreach (var _cabe in listRubrosCabecera)
                    {
                        //workSheet.Cells["A" + fila + ":H" + fila].Merge = true;
                        workSheet.Cells["A" + fila + ":G" + fila].Style.Fill.PatternType = ExcelFillStyle.Solid; workSheet.Cells["A" + fila + ":G" + fila].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(150, 150, 150));
                        workSheet.Cells["A" + fila].Value = _cabe.descripcion.TrimEnd(); workSheet.Cells["A" + fila].Style.Font.Bold = true;

                        foreach (var _granRub in listGrandesRubros)
                        {
                            var TieneEliminaciones = (from _eli in ListaEliminaciones
                                                      where _eli.RubcabCodigo == _cabe.codigo && _eli.GranrubCodigo == _granRub.codigo
                                                      select _eli);

                            if (TieneEliminaciones.Count() > 0)
                            {
                                fila += 1;
                                //workSheet.Cells["A" + fila + ":H" + fila].Merge = true;
                                workSheet.Cells["A" + fila + ":G" + fila].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells["A" + fila + ":G" + fila].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 240, 240));
                                workSheet.Cells["A" + fila].Value = _granRub.descripcion.TrimEnd(); workSheet.Cells["A" + fila].Style.Font.Bold = true;

                                foreach (var _subRub in listSubRubros)
                                {
                                    var eliminacionesBySubRubro = (from _eli in ListaEliminaciones
                                                                   where _eli.RubcabCodigo == _cabe.codigo && _eli.GranrubCodigo == _granRub.codigo && _eli.SubrubId == _subRub.codigo
                                                                   select new
                                                                   {
                                                                       rubro = _subRub.descripcion,
                                                                       origen = _eli.EmpDescripcion.Trim(),
                                                                       contraparte = _eli.EmpDescripcionContraparte.Trim(),
                                                                       //concepto = string.Empty, // ????
                                                                       bcra = Convert.ToInt32(_eli.RubCodigo),
                                                                       moneda = _eli.MonDescripcion,
                                                                       saldo = Convert.ToDecimal(((decimal)_eli.EliSaldo / 1000)),
                                                                       saldoPromedio = Convert.ToDecimal(((decimal)_eli.EliSaldoPromedio / 1000)),
                                                                   });

                                    if (eliminacionesBySubRubro.Count() > 0)
                                    {
                                        fila += 1;
                                        workSheet.Cells["A" + fila].LoadFromCollection(eliminacionesBySubRubro);
                                        fila += eliminacionesBySubRubro.Count();
                                        //workSheet.Cells["A" + fila + ":F" + fila].Merge = true;
                                        workSheet.Cells["A" + fila].Value = "   TOTAL " + _subRub.descripcion.TrimEnd() + ":"; workSheet.Cells["A" + fila].Style.Font.Bold = true; workSheet.Cells["A" + fila].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                        // calculo los totales
                                        totalSubRubros = eliminacionesBySubRubro.Sum(item => item.saldo);
                                        totalSubRubrosProm = eliminacionesBySubRubro.Sum(item => item.saldoPromedio);
                                        workSheet.Cells["F" + fila].Value = totalSubRubros; workSheet.Cells["F" + fila].Style.Font.Bold = true;
                                        workSheet.Cells["G" + fila].Value = totalSubRubrosProm; workSheet.Cells["G" + fila].Style.Font.Bold = true;
                                        // acumulo el total del gran rubro
                                        totalGrandesRubros += totalSubRubros;
                                        totalGrandesRubrosProm += totalSubRubrosProm;

                                    }
                                }

                                //workSheet.Cells["A" + (fila + 1) + ":H" + (fila + 1)].Merge = true;
                                fila += 2;
                                workSheet.Cells["A" + fila + ":G" + fila].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells["A" + fila + ":G" + fila].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 240, 240));
                                //workSheet.Cells["A" + fila + ":F" + fila].Merge = true;
                                workSheet.Cells["A" + fila].Value = "TOTAL " + _granRub.descripcion.TrimEnd() + ":"; workSheet.Cells["A" + fila].Style.Font.Bold = true; workSheet.Cells["A" + fila].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                workSheet.Cells["F" + fila].Value = totalGrandesRubros; workSheet.Cells["F" + fila].Style.Font.Bold = true;
                                workSheet.Cells["G" + fila].Value = totalGrandesRubrosProm; workSheet.Cells["G" + fila].Style.Font.Bold = true;


                                // acumulo el total del activo
                                totalCabecera += totalGrandesRubros;
                                totalCabeceraProm += totalGrandesRubrosProm;
                                // limpio los totales de los grandes rubros
                                totalGrandesRubros = 0;
                                totalGrandesRubrosProm = 0;

                                fila += 1;
                                workSheet.Cells["A" + fila + ":G" + fila].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                //workSheet.Cells["A" + fila + ":H" + fila].Merge = true;

                            }
                        }

                        fila += 2;
                        //workSheet.Cells["A" + (fila - 1) + ":H" + (fila - 1)].Merge = true;
                        workSheet.Cells["A" + fila + ":G" + fila].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells["A" + fila + ":G" + fila].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(150, 150, 150));
                        //workSheet.Cells["A" + fila + ":F" + fila].Merge = true;
                        workSheet.Cells["A" + fila].Value = "TOTAL " + _cabe.descripcion.TrimEnd() + ":"; workSheet.Cells["A" + fila].Style.Font.Bold = true; workSheet.Cells["A" + fila].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        workSheet.Cells["F" + fila].Value = totalCabecera; workSheet.Cells["F" + fila].Style.Font.Bold = true;
                        workSheet.Cells["G" + fila].Value = totalCabeceraProm; workSheet.Cells["G" + fila].Style.Font.Bold = true;
                        // acumulo el total general
                        totalGeneral += totalCabecera;
                        totalGeneralProm += totalCabeceraProm;
                        // limpio el total de la cabecera
                        totalCabecera = 0;
                        totalCabeceraProm = 0;
                        fila += 1;
                        //workSheet.Cells["A" + fila + ":H" + fila].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        //workSheet.Cells["A" + fila + ":H" + fila].Merge = true;
                        fila += 1;
                    }

                    // Muestro el total general
                    fila += 3;
                    //workSheet.Cells["A" + (fila - 1) + ":H" + (fila - 1)].Merge = true;
                    //workSheet.Cells["A" + (fila - 2) + ":H" + (fila - 2)].Merge = true;
                    workSheet.Cells["A" + fila + ":G" + fila].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["A" + fila + ":G" + fila].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));
                    //workSheet.Cells["A" + fila + ":F" + fila].Merge = true;
                    workSheet.Cells["A" + fila].Value = "TOTAL GENERAL:"; workSheet.Cells["A" + fila].Style.Font.Bold = true; workSheet.Cells["A" + fila].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; workSheet.Cells["A" + fila].Style.Font.Size = 14;
                    workSheet.Cells["F" + fila].Value = totalGeneral; workSheet.Cells["F" + fila].Style.Font.Bold = true;
                    workSheet.Cells["G" + fila].Value = totalGeneralProm; workSheet.Cells["G" + fila].Style.Font.Bold = true;
                    workSheet.Cells["A" + fila + ":G" + fila].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["A" + fila + ":G" + fila].Style.Border.Bottom.Style = ExcelBorderStyle.Double;


                    // Estilos Columnas
                    workSheet.Column(1).Width = 60;
                    workSheet.Column(2).Width = 35;
                    workSheet.Column(3).Width = 35;
                    //workSheet.Column(4).Width = 30; //workSheet.Column(4).Style.WrapText = true;
                    workSheet.Column(4).Width = 10; //workSheet.Column(5).Style.WrapText = true;
                    workSheet.Column(5).Width = 8; //workSheet.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Column(6).Width = 18; workSheet.Column(6).Style.Numberformat.Format = "#,##"; //"#,##0.00";
                    workSheet.Column(7).Width = 18; workSheet.Column(7).Style.Numberformat.Format = "#,##";

                    package.Save();
                }

                stream.Position = 0;
                string excelName = $"Eliminaciones-{DateTime.Now.ToString("yyyyMMdd_HHmmssfff")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private RelacionRubrosDTO ObtenerRelacionRubro(string Rubro)
        {
            try
            {
                return (from _rel in _context.RelacionRubros
                        where _rel.RubCodigo == Rubro
                        select new RelacionRubrosDTO
                        {
                            RubcabCodigo = _rel.RubcabCodigo,
                            RubcabDescripcion = (from rc in _context.RubrosCabecera where rc.RubcabCodigo == _rel.RubcabCodigo select rc.RubcabDescripcion).First().ToString(),
                            GranrubCodigo = _rel.GranrubCodigo,
                            GranrubDescripcion = (from gr in _context.GrandesRubros where gr.GranrubCodigo == _rel.GranrubCodigo select gr.GranrubDescripcion).First().ToString(),
                            SubrubId = _rel.SubrubId,
                            SubrubDescripcion = (from sr in _context.SubRubros where sr.SubrubId == _rel.SubrubId select sr.SubrubDescripcion).First().ToString(),
                            RubCodigo = _rel.RubCodigo
                        }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IQueryable<EliminacionesDTO> ObtenerEliminaciones(string Periodo, int Sec_Codigo)
        {
            try
            {
                //var ListEmpbyGrupo = (from gemp in _context.GrupoEmpresas where gemp.GrupoId == GrupoId && gemp.SecCodigo == Sec_Codigo select gemp.EmpCodigo).ToList();

                return (from _eli in _context.Eliminaciones
                        where _eli.Periodo == Periodo && _eli.SecCodigo == Sec_Codigo //&& ListEmpbyGrupo.Contains(_eli.EmpCodigo)
                        select new EliminacionesDTO
                        {
                            EmpCodigo = _eli.EmpCodigo,
                            EmpDescripcion = (from emp in _context.Empresas where emp.EmpCodigo == _eli.EmpCodigo && emp.SecCodigo == Sec_Codigo select emp.EmpDescripcion).First().ToString(),
                            Periodo = _eli.Periodo,
                            EmpCodigoContraparte = _eli.EmpCodigoContraparte,
                            EmpDescripcionContraparte = (from empC in _context.Empresas where empC.EmpCodigo == _eli.EmpCodigoContraparte && empC.SecCodigo == Sec_Codigo select empC.EmpIntercompany).First().ToString(),
                            RubCodigo = _eli.RubCodigo,
                            RubDescripcion = (from rub in _context.Rubros where rub.RubCodigo == _eli.RubCodigo select rub.RubDescripcion).First().ToString(),
                            EliSaldo = _eli.EliSaldo,
                            EliSaldoPromedio = _eli.EliSaldoPromedio,
                            EmpPorcentaje = _eli.EmpPorcentaje,
                            MonCodigo = _eli.MonCodigo,
                            MonDescripcion = (from mon in _context.Moneda where mon.MonCodigo == _eli.MonCodigo select mon.MonDescripcion).First().ToString(),
                            SecCodigo = _eli.SecCodigo,
                            SecDescripcion = (from sec in _context.Sectores where sec.SecCodigo == _eli.SecCodigo select sec.SecDescripcion).First().ToString(),
                            EsAjuste = _eli.EsAjuste
                        });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IQueryable<ReporteEliminacionesDTO> ObtenerReporteEliminaciones(string Periodo, int Sec_Codigo)
        {
            try
            {
                var result = (from _eli in _context.Eliminaciones
                              join _rel in _context.RelacionRubros on _eli.RubCodigo equals _rel.RubCodigo
                              where _eli.Periodo == Periodo && _eli.SecCodigo == Sec_Codigo && _rel.Activo == "S"
                              select new ReporteEliminacionesDTO
                              {
                                  EmpCodigo = _eli.EmpCodigo,
                                  EmpDescripcion = ((from emp in _context.Empresas where emp.EmpCodigo == _eli.EmpCodigo && emp.SecCodigo == Sec_Codigo select emp.EmpDescripcion).First().Count() > 0) ? (from emp in _context.Empresas where emp.EmpCodigo == _eli.EmpCodigo && emp.SecCodigo == Sec_Codigo select emp.EmpDescripcion.TrimEnd()).First().ToString() : null,
                                  Periodo = _eli.Periodo,
                                  EmpCodigoContraparte = _eli.EmpCodigoContraparte,
                                  EmpDescripcionContraparte = ((from empC in _context.Empresas where empC.EmpCodigo == _eli.EmpCodigoContraparte && empC.SecCodigo == Sec_Codigo select empC.EmpIntercompany).First().Count() > 0) ? (from empC in _context.Empresas where empC.EmpCodigo == _eli.EmpCodigoContraparte && empC.SecCodigo == Sec_Codigo select empC.EmpIntercompany.TrimEnd()).First().ToString() : null,
                                  RubCodigo = _eli.RubCodigo,
                                  RubDescripcion = ((from rub in _context.Rubros where rub.RubCodigo == _eli.RubCodigo select rub.RubDescripcion).First().Count() > 0) ? (from rub in _context.Rubros where rub.RubCodigo == _eli.RubCodigo select rub.RubDescripcion.TrimEnd()).First().ToString() : null,
                                  EliSaldo = _eli.EliSaldo,
                                  EliSaldoPromedio = _eli.EliSaldoPromedio,
                                  EmpPorcentaje = _eli.EmpPorcentaje,
                                  MonCodigo = _eli.MonCodigo,
                                  MonDescripcion = ((from mon in _context.Moneda where mon.MonCodigo == _eli.MonCodigo select mon.MonDescripcion).First().Count() > 0) ? (from mon in _context.Moneda where mon.MonCodigo == _eli.MonCodigo select mon.MonDescripcion.TrimEnd()).First().ToString() : null,
                                  SecCodigo = _eli.SecCodigo,
                                  SecDescripcion = ((from sec in _context.Sectores where sec.SecCodigo == _eli.SecCodigo select sec.SecDescripcion).First().Count() > 0) ? (from sec in _context.Sectores where sec.SecCodigo == _eli.SecCodigo select sec.SecDescripcion.TrimEnd()).First().ToString() : null,
                                  EsAjuste = _eli.EsAjuste,
                                  RubcabCodigo = _rel.RubcabCodigo,
                                  RubcabDescripcion = ((from rc in _context.RubrosCabecera where rc.RubcabCodigo == _rel.RubcabCodigo select rc.RubcabDescripcion).First().Count() > 0) ? (from rc in _context.RubrosCabecera where rc.RubcabCodigo == _rel.RubcabCodigo select rc.RubcabDescripcion.TrimEnd()).First().ToString() : null,
                                  GranrubCodigo = _rel.GranrubCodigo,
                                  GranrubDescripcion = ((from gr in _context.GrandesRubros where gr.GranrubCodigo == _rel.GranrubCodigo select gr.GranrubDescripcion).First().Count() > 0) ? (from gr in _context.GrandesRubros where gr.GranrubCodigo == _rel.GranrubCodigo select gr.GranrubDescripcion.TrimEnd()).First().ToString() : null,
                                  SubrubId = _rel.SubrubId,
                                  SubrubDescripcion = ((from sr in _context.SubRubros where sr.SubrubId == _rel.SubrubId select sr.SubrubDescripcion).First().Count() > 0) ? (from sr in _context.SubRubros where sr.SubrubId == _rel.SubrubId select sr.SubrubDescripcion.TrimEnd()).First().ToString() : null,

                              });

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






    }
}
