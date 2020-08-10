using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using VigCovidApp.Controllers.Base;
using VigCovidApp.Models;
using VigCovidApp.Utils.Export;
using VigCovidApp.ViewModels;
using static VigCovidApp.Models.Enums;

namespace VigCovidApp.Controllers
{
    public class MonitoringWorkerController : GenericController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult RegisterMonitoring(int id)
        {
            //ViewBag.COMBOCALIFICACION = ComboCalificacion();
            ViewBag.REGISTROTRABAJADOR = id;

            ViewBag.INDICADORES = GetIndicadores(id);

            ViewBag.EXAMENES = ListarExamenesTrabajador(id);

            var seguimientos = Seguimientos(id);
            if (seguimientos.Count() == 0) return View(new List<SeguimientosViewModel>());

            return View(seguimientos);
        }

        [HttpGet]
        public ActionResult NewRegisterMonitoring(int id)
        {
            try
            {
                #region Obtener Nro Seguimiento siguiente
                var nroSeguimientoSiguiente = GetNroMonitoring(id) + 1;
                #endregion

                #region Validar que se creé un seguimiento al día
                if (ValidateMonitoring(id) != null) return null;
                #endregion

                #region Grabar Nuevo Seguimiento
                var oSeguimiento = new Seguimiento();
                oSeguimiento.RegistroTrabajadorId = id;
                oSeguimiento.Fecha = DateTime.Now.Date;
                oSeguimiento.ClasificacionId = -1;

                oSeguimiento.SensacionFiebre = false;
                oSeguimiento.Tos = false;
                oSeguimiento.DolorGarganta = false;
                oSeguimiento.DificultadRespiratoria = false;

                oSeguimiento.CongestionNasal = false;
                oSeguimiento.Cefalea = false;
                oSeguimiento.DolorMuscular = false;
                oSeguimiento.PerdidaOlfato = false;

                oSeguimiento.Comentario = "";
                //oSeguimiento.ResultadoCovid19 = 1;
                oSeguimiento.TipoEstadoId = -1;
                oSeguimiento.NroSeguimiento = nroSeguimientoSiguiente;
                db.Seguimiento.Add(oSeguimiento);
                db.SaveChanges();
                #endregion

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult UpdateSeguimiento(ActualizarSeguimientoViewModel entidad)
        {
            try
            {
                var result = db.Seguimiento.SingleOrDefault(b => b.Id == entidad.Id);
                if (result != null)
                {
                    //result.ClasificacionId  = entidad.ClasificacionId;
                    result.SensacionFiebre = entidad.SensacionFiebre;
                    result.Tos = entidad.Tos;
                    result.DolorGarganta = entidad.DolorGarganta;
                    result.DificultadRespiratoria = entidad.DificultadRespiratoria;

                    result.CongestionNasal = entidad.CongestionNasal;
                    result.Cefalea = entidad.Cefalea;
                    result.DolorMuscular = entidad.DolorMuscular;
                    result.PerdidaOlfato = entidad.PerdidaOlfato;
                    //--------------------------------------------------
                    result.HipertensionArterial = entidad.HipertensionArterial;
                    result.HipertensionArterialNoControlada = entidad.HipertensionArterialNoControlada;
                    result.AsmaModeradoSevero = entidad.AsmaModeradoSevero;
                    result.Diabetes = entidad.Diabetes;
                    result.Mayor65 = entidad.Mayor65;
                    result.Cancer = entidad.Cancer;
                    result.CardiovascularGrave = entidad.CardiovascularGrave;
                    result.ImcMayor40 = entidad.ImcMayor40;
                    result.RenalDialisis = entidad.RenalDialisis;
                    result.PulmonarCronica = entidad.PulmonarCronica;
                    result.TratInmunosupresor = entidad.TratInmunosupresor;

                    result.CasoPositivo = entidad.CasoPositivo;
                    result.CasoSospechoso = entidad.CasoSospechoso;
                    result.RinofaringitisAguda = entidad.RinofaringitisAguda;
                    result.NeumoniaViral = entidad.NeumoniaViral;
                    result.ContactoEnfermedades = entidad.ContactoEnfermedades;
                    result.Aislamiento = entidad.Aislamiento;
                    result.Otros = entidad.Otros;
                    result.OtrosComentar = entidad.OtrosComentar;

                    result.Comentario = entidad.Comentario;
                    //result.ResultadoCovid19 = entidad.ResultadoCovid19;
                    //result.FechaResultadoCovid19 = entidad.FechaResultadoCovid19;
                    result.TipoEstadoId = entidad.TipoEstadoId;
                    db.SaveChanges();
                }

                return Json(result, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult UpdateFechaSeguimiento(FechaSeguimiento entidad)
        {
            try
            {
                #region Validar que se creé un seguimiento al día
                if (ValidateMonitoring(entidad.TrabajadorId, entidad.Fecha) != null) return null;
                #endregion

                var result = db.Seguimiento.SingleOrDefault(b => b.Id == entidad.SeguimientoId);
                if (result != null)
                {
                    result.Fecha = entidad.Fecha;
                    db.SaveChanges();
                }

                return Json(result, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult UpdateEstadoClinico(EstadoClinico oEstadoClinico)
        {
            try
            {
                var result = db.RegistroTrabajador.SingleOrDefault(b => b.Id == oEstadoClinico.TrabajadorId);
                if (result != null)
                {
                    result.EstadoClinicoId = (int)Enums.EstadoClinico.alta;
                    db.SaveChanges();
                }

                return Json(result, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<SelectListItem> ComboCalificacion()
        {
            List<SelectListItem> opcionesCalificacion = new List<SelectListItem>();
            opcionesCalificacion.Add(new SelectListItem { Text = " ", Value = "-1" });
            opcionesCalificacion.Add(new SelectListItem { Text = "Asintomático", Value = "1" });
            opcionesCalificacion.Add(new SelectListItem { Text = "Sintomático Leve", Value = "2" });
            opcionesCalificacion.Add(new SelectListItem { Text = "Sintomático Moderado", Value = "3" });

            return opcionesCalificacion;
        }

        #region Private Methodos 
        private IEnumerable<SeguimientosViewModel> Seguimientos(int registroTrabajadorId)
        {
            try
            {
                var listaSeguimientos = new List<SeguimientosViewModel>();
                var seguimientos = db.Seguimiento.Where(w => w.RegistroTrabajadorId == registroTrabajadorId).ToList().OrderByDescending(p => p.NroSeguimiento);

                foreach (var seguimiento in seguimientos)
                {
                    var oSeguimiento = new SeguimientosViewModel();
                    oSeguimiento.SeguimientoId = seguimiento.Id;
                    oSeguimiento.RegistroTrabajadorId = seguimiento.RegistroTrabajadorId;
                    oSeguimiento.Fecha = seguimiento.Fecha.Date.ToString("dd-MMMM-yyy");
                    oSeguimiento.ClasificacionId = seguimiento.ClasificacionId;
                    oSeguimiento.SensacionFiebre = seguimiento.SensacionFiebre == true ? "checked" : "";
                    oSeguimiento.Tos = seguimiento.Tos == true ? "checked" : "";
                    oSeguimiento.DolorGarganta = seguimiento.DolorGarganta == true ? "checked" : "";
                    oSeguimiento.DificultadRespiratoria = seguimiento.DificultadRespiratoria == true ? "checked" : "";

                    oSeguimiento.CongestionNasal = seguimiento.CongestionNasal == true ? "checked" : "";
                    oSeguimiento.Cefalea = seguimiento.Cefalea == true ? "checked" : "";
                    oSeguimiento.DolorMuscular = seguimiento.DolorMuscular == true ? "checked" : "";
                    oSeguimiento.PerdidaOlfato = seguimiento.PerdidaOlfato == true ? "checked" : "";

                    oSeguimiento.Comentario = seguimiento.Comentario;
                    //oSeguimiento.ResultadoCovid19 = seguimiento.ResultadoCovid19.Value;
                    //oSeguimiento.FechaResultadoCovid19 = seguimiento.FechaResultadoCovid19 < DateTime.Now.AddYears(-5) ? null : seguimiento.FechaResultadoCovid19;
                    oSeguimiento.TipoEstadoId = seguimiento.TipoEstadoId.Value;
                    oSeguimiento.NroSeguimiento = seguimiento.NroSeguimiento;
                    //-----------------------------------------------------
                    oSeguimiento.HipertensionArterial = seguimiento.HipertensionArterial == true ? "checked" : "";
                    oSeguimiento.HipertensionArterialNoControlada = seguimiento.HipertensionArterialNoControlada == true ? "checked" : "";
                    oSeguimiento.AsmaModeradoSevero = seguimiento.AsmaModeradoSevero == true ? "checked" : "";
                    oSeguimiento.Diabetes = seguimiento.Diabetes == true ? "checked" : "";
                    oSeguimiento.Mayor65 = seguimiento.Mayor65 == true ? "checked" : "";
                    oSeguimiento.Cancer = seguimiento.Cancer == true ? "checked" : "";
                    oSeguimiento.CardiovascularGrave = seguimiento.CardiovascularGrave == true ? "checked" : "";
                    oSeguimiento.ImcMayor40 = seguimiento.ImcMayor40 == true ? "checked" : "";
                    oSeguimiento.RenalDialisis = seguimiento.RenalDialisis == true ? "checked" : "";
                    oSeguimiento.PulmonarCronica = seguimiento.PulmonarCronica == true ? "checked" : "";
                    oSeguimiento.TratInmunosupresor = seguimiento.TratInmunosupresor == true ? "checked" : "";

                    oSeguimiento.CasoPositivo = seguimiento.CasoPositivo == true ? "checked" : "";
                    oSeguimiento.CasoSospechoso = seguimiento.CasoSospechoso == true ? "checked" : "";
                    oSeguimiento.RinofaringitisAguda = seguimiento.RinofaringitisAguda == true ? "checked" : "";
                    oSeguimiento.NeumoniaViral = seguimiento.NeumoniaViral == true ? "checked" : "";
                    oSeguimiento.ContactoEnfermedades = seguimiento.ContactoEnfermedades == true ? "checked" : "";
                    oSeguimiento.Aislamiento = seguimiento.Aislamiento == true ? "checked" : "";
                    oSeguimiento.Otros = seguimiento.Otros == true ? "checked" : "";
                    oSeguimiento.OtrosComentar = seguimiento.OtrosComentar;
                    listaSeguimientos.Add(oSeguimiento);
                }

                return listaSeguimientos;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private int GetNroMonitoring(int registroTrabajadorId)
        {

            var seguimientos = db.Seguimiento.Where(w => w.RegistroTrabajadorId == registroTrabajadorId).OrderByDescending(o => o.NroSeguimiento).ToList();
            if (seguimientos.Count == 0)
                return 0;

            return seguimientos[0].NroSeguimiento;
        }
        private Seguimiento ValidateMonitoring(int registroTrabajadorId)
        {
            var fechaHoy = DateTime.Now.Date;
            var result = db.Seguimiento.Where(p => p.RegistroTrabajadorId == registroTrabajadorId && p.Fecha == fechaHoy).FirstOrDefault();

            return result;
        }
        private Seguimiento ValidateMonitoring(int registroTrabajadorId, DateTime fecha)
        {
            var result = db.Seguimiento.Where(p => p.RegistroTrabajadorId == registroTrabajadorId && p.Fecha == fecha).FirstOrDefault();

            return result;
        }
        private IndicadoresViewModel GetIndicadores(int registroTrabajadorId)
        {
            var result = new IndicadoresViewModel();
            var seguimientos = db.Seguimiento.Where(w => w.RegistroTrabajadorId == registroTrabajadorId).ToList().OrderByDescending(p => p.NroSeguimiento).ToList();
            var datosRegistro = db.RegistroTrabajador.Where(w => w.Id == registroTrabajadorId).FirstOrDefault();

            result.Trabajador = datosRegistro.NombreCompleto;
            result.Empresa = GetEmpresa(datosRegistro.EmpresaCodigo);
            result.Celular = datosRegistro.Celular;
            result.Email = datosRegistro.Email;
            if (seguimientos.Count > 0)
            {
                result.FechaRegistro = seguimientos.Find(p => p.NroSeguimiento == 1).Fecha.ToString("dd/MM/yyyy");
                result.DiasEnSeguimiento = CalcularDiasSeguimiento(seguimientos); //(seguimientos.Count().ToString();
                result.FechaInicioSintomas = GetFechaInicioSintomas(seguimientos);
                result.NroDiasSinSintomas = (GetNroDiasSinSintomas(seguimientos)).ToString();
                result.NroDiasPrueba = GetDiasPrueba(seguimientos).Contador.ToString();
                result.ResultadoPrueba = GetDiasPrueba(seguimientos).Resultado;
            }
            else
            {
                result.FechaRegistro = "";
                result.DiasEnSeguimiento = "";
                result.FechaInicioSintomas = "";
                result.NroDiasSinSintomas = "";
                result.NroDiasPrueba = "";
                result.ResultadoPrueba = "";
            }

            return result;
        }

        private string CalcularDiasSeguimiento(List<Seguimiento> seguimientos)
        {
            var primerSeguimiento = seguimientos.Find(p => p.NroSeguimiento == 1).Fecha;

            var dias = (DateTime.Now - primerSeguimiento).TotalDays;
            var rDias = Decimal.Parse(dias.ToString());
            
            return Decimal.Round(rDias).ToString();  
        }

        private IndicadorCovid19 GetDiasPrueba(List<Seguimiento> seguimientos)
        {
            var oIndicadorCovid19 = new IndicadorCovid19();
            var count = 0;
            var resultado = "";

            var sortSeguimientos = seguimientos.OrderByDescending(p => p.NroSeguimiento);

            foreach (var item in sortSeguimientos)
            {
                //if (!(item.FechaResultadoCovid19 == null || item.FechaResultadoCovid19 < DateTime.Now.AddYears(-5))
                //&& (!(item.ResultadoCovid19 == null || item.ResultadoCovid19 == -1)))
                //{
                //    resultado = GetResultadoCovid19(item.ResultadoCovid19.Value);
                //    break;
                //}
                //else
                //{
                //    count++;
                //}

            }
            oIndicadorCovid19.Contador = count;
            oIndicadorCovid19.Resultado = resultado;
            return oIndicadorCovid19;
        }
        private int GetNroDiasSinSintomas(List<Seguimiento> seguimientos)
        {
            var count = 0;

            foreach (var item in seguimientos)
            {
                if (item.SensacionFiebre == false
                    && item.Tos == false
                    && item.DolorGarganta == false
                    && item.DificultadRespiratoria == false
                    && item.CongestionNasal == false
                    && item.Cefalea == false
                    && item.DolorMuscular == false
                    && item.PerdidaOlfato == false)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count;
        }
        private string GetFechaInicioSintomas(List<Seguimiento> seguimientos)
        {
            var sortSeguimientos = seguimientos.OrderBy(p => p.NroSeguimiento);
            var fechaInicioSintomas = "";
            foreach (var item in sortSeguimientos)
            {
                if (item.SensacionFiebre == true
                    || item.Tos == true
                    || item.DolorGarganta == true
                    || item.DificultadRespiratoria == true
                    || item.CongestionNasal == true
                    || item.Cefalea == true
                    || item.DolorMuscular == true
                    || item.PerdidaOlfato == true)
                {
                    fechaInicioSintomas = item.Fecha.ToString("dd/MM/yyyy");
                    break;
                }
            }

            return fechaInicioSintomas;

        }
        private string GetEmpresa(int id)
        {
            if (id == 1)
            {
                return "SALUS LABORIS";
            }
            else if (id == 2)
            {
                return "BACKUS";
            }
            else if (id == 3)
            {
                return "AMBEV";
            }
            else
            {
                return "";
            }
        }
        private string GetResultadoCovid19(int id)
        {
            if (id == (int)ResultadoCovid19.Negativo)
            {
                return "Negativo";
            }
            else if (id == (int)ResultadoCovid19.Novalido)
            {
                return "No válido";
            }
            else if (id == (int)ResultadoCovid19.IgMPositivo)
            {
                return "IgM Positivo";
            }
            else if (id == (int)ResultadoCovid19.IgGPositivo)
            {
                return "IgG Positivo";
            }
            else if (id == (int)ResultadoCovid19.IgMeIgGpositivo)
            {
                return "IgM e IgG positivo";
            }
            else if (id == (int)ResultadoCovid19.Noserealizo)
            {
                return "No se realizó";
            }
            else
            {
                return "";
            }
        }
        private List<ListarExamenesViewModel> ListarExamenesTrabajador(int trabajadorId)
        {
            var query = (from A in db.Examen where A.TrabajadorId == trabajadorId select A).ToList();

            var examenes = new List<ListarExamenesViewModel>();
            foreach (var item in query)
            {
                var examen = new ListarExamenesViewModel();
                examen.IdExamen = item.Id;
                examen.IdTrabajador = item.TrabajadorId;
                examen.FechaExamen = item.Fecha;
                examen.TipoPrueba = item.TipoPrueba == 0 ? "Prueba rápida" : "Molecular";
                examen.Resultado = ObtenerResultado(item.Resultado);

                examenes.Add(examen);
            }

            return examenes;
        }
        private string ObtenerResultado(int resultado)
        {
            if (resultado == 0)
            {
                return "Negativo";
            }
            else if (resultado == 1)
            {
                return "No válido";
            }
            else if (resultado == 2)
            {
                return "IgM Positivo";
            }
            else if (resultado == 3)
            {
                return "IgG Positivo";
            }
            else if (resultado == 4)
            {
                return "IgM e IgG Positivo";
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region Class

        class IndicadorCovid19
        {
            public int Contador { get; set; }
            public string Resultado { get; set; }
        }

        public class FechaSeguimiento
        {
            public int TrabajadorId { get; set; }
            public int SeguimientoId { get; set; }
            public DateTime Fecha { get; set; }
        }

        public class EstadoClinico
        {
            public int TrabajadorId { get; set; }
        }

        #endregion

        public FileResult ExportAltaMedica()
        {
            MemoryStream memoryStream = GetPdfAltaMedica();

            string fileName = string.Empty;
            DateTime fileCreationDatetime = DateTime.Now;

            return File(memoryStream, "application/pdf", fileName);
        }

        private MemoryStream GetPdfAltaMedica()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (Document document = new Document(PageSize.A4, 40, 40, 140, 40))
            {
                try
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(document, memoryStream);
                    pdfWriter.CloseStream = false;
                    pdfWriter.PageEvent = new ITextEvents();

                    document.Open();

                    document.Close();

                    byte[] byteInfo = memoryStream.ToArray();
                    memoryStream.Write(byteInfo, 0, byteInfo.Length);
                    memoryStream.Position = 0;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return memoryStream;
        }

    }
}