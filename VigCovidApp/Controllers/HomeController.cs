using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VigCovidApp.Controllers.Base;
using VigCovidApp.Models;
using VigCovidApp.ViewModels;
using static VigCovidApp.Models.Enums;

namespace VigCovidApp.Controllers
{
    public class HomeController : GenericController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var sessione = (SessionModel)Session[Resources.Constants.SessionUser];
            var empresasAsignadas = sessione.EmpresasAsignadas;
            var empresasCodigos = new List<int>();
            foreach (var item in empresasAsignadas)
            {
                empresasCodigos.Add(item.Codigo);
            }

            ViewBag.EMPRESASASIGANADAS = empresasAsignadas;

            var listaTrabajadores = ListaTrabajadores(empresasAsignadas[0].Codigo).ToList().FindAll(p => p.EstadoClinicoId != 1);
            return View(listaTrabajadores);          
        }

        [HttpPost]
        public ActionResult RegistrarTrabajador(RegistroTrabajador registrarTrabajador)
        {
            try
            {
                registrarTrabajador.FechaIngreso = DateTime.Now.Date;
                db.RegistroTrabajador.Add(registrarTrabajador);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            catch (Exception ex)
            {
                return null;
            }                
        }

        #region Private Methodos 

        private IEnumerable<ListaTrabajadoresViewModel> ListaTrabajadores(List<int> EmpresasCodigos)
        {
            var listaTrabajadores = db.RegistroTrabajador.Where(r => EmpresasCodigos.Contains(r.EmpresaCodigo)).ToList();
            var fechaHoy = DateTime.Now.Date;
            var listaSeguimientosHoy = db.Seguimiento.Where(p => p.Fecha == fechaHoy).ToList();

            var query = (from A in listaTrabajadores
                    select new ListaTrabajadoresViewModel
                    {
                        RegistroTrabajadorId = A.Id,
                        NombreCompleto = A.NombreCompleto,
                        Dni = A.Dni,
                        Edad = A.Edad.ToString(),
                        FechaIngreso = A.FechaIngreso.ToString("dd/MM/yyyy"),
                        ModoIngreso = (ModoIngreso)A.ModoIngreso,
                        EstadoDiario = "",
                    }).ToList();

            return (from A in query
                    select new ListaTrabajadoresViewModel
                    {
                        RegistroTrabajadorId = A.RegistroTrabajadorId,
                        NombreCompleto = A.NombreCompleto,
                        Dni = A.Dni,
                        Edad = A.Edad.ToString(),
                        FechaIngreso = A.FechaIngreso,
                        ModoIngreso = A.ModoIngreso,                        
                        HistorialEnfermedad = A.HistorialEnfermedad,
                        EstadoDiario = listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId) == null ?"" : ObtenerNombreEstado(listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId).TipoEstadoId.Value),
                        Clasificacion = listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId) == null ? "": ObtenerNombreCalificacion(listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId).ClasificacionId),
                        UltimoComentario = listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId) == null ? "" : listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId).Comentario
                        //UltimoSeguimiento = listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId) == null?"" : listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId).Fecha.ToString("dd-MM-yyyy"),
                    });
        }

        private IEnumerable<ListaTrabajadoresViewModel> ListaTrabajadores(int EmpresaCodigo)
        {
            var listaTrabajadores = db.RegistroTrabajador.Where(r => r.EmpresaCodigo == EmpresaCodigo).ToList();
            var fechaHoy = DateTime.Now.Date;
            var listaSeguimientosHoy = db.Seguimiento.Where(p => p.Fecha == fechaHoy).ToList();

            var query = (from A in listaTrabajadores
                         select new ListaTrabajadoresViewModel
                         {
                             RegistroTrabajadorId = A.Id,
                             NombreCompleto = A.ApePaterno + " " + A.ApeMaterno +", "+ A.NombreCompleto,
                             Dni = A.Dni,
                             Edad = A.Edad.ToString(),
                             FechaIngreso = A.FechaIngreso.ToString("dd/MM/yyyy"),
                             ModoIngreso = (ModoIngreso)A.ModoIngreso,
                             EstadoDiario = "",
                             EstadoClinicoId = A.EstadoClinicoId
                         }).ToList();

            return (from A in query
                    select new ListaTrabajadoresViewModel
                    {
                        RegistroTrabajadorId = A.RegistroTrabajadorId,
                        NombreCompleto = A.NombreCompleto,
                        Dni = A.Dni,
                        Edad = A.Edad.ToString(),
                        FechaIngreso = A.FechaIngreso,
                        ModoIngreso = A.ModoIngreso,
                        HistorialEnfermedad = A.HistorialEnfermedad,
                        EstadoDiario = listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId) == null ? "" : ObtenerNombreEstado(listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId).TipoEstadoId.Value),
                        Clasificacion = listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId) == null ? "" : ObtenerNombreCalificacion(listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId).ClasificacionId),
                        UltimoComentario = listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId) == null ? "" : listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId).Comentario,
                        UltimoSeguimiento = listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId) == null ? "" : listaSeguimientosHoy.Find(p => p.RegistroTrabajadorId == A.RegistroTrabajadorId).Fecha.ToString("dd-MM-yyyy"),
                        Alerta = GetNroDiasSinSintomas(A.RegistroTrabajadorId),
                        ContadorSeguimiento = GetNroMonitoring(A.RegistroTrabajadorId).ToString(),
                        DiaSinSintomas = GetNumeroDiasSinSintomas(A.RegistroTrabajadorId).ToString(),
                        EstadoClinicoId = A.EstadoClinicoId
                    }) ;
        }

        //private string ObtenerNombreClasificacionHoy(int RegistroTrabajadorId)
        //{
        //    var hoy = DateTime.Now.Date;
        //    var calificacionHoy = db.Seguimiento.Where(w => w.Fecha == hoy && w.RegistroTrabajadorId == RegistroTrabajadorId).FirstOrDefault();
        //    if (calificacionHoy != null)
        //        return ObtenerNombreCalificacion(calificacionHoy.ClasificacionId);

        //    return "Sin calificación";
        //}

        private int GetNroMonitoring(int registroTrabajadorId)
        {

            var seguimientos = db.Seguimiento.Where(w => w.RegistroTrabajadorId == registroTrabajadorId).OrderByDescending(o => o.NroSeguimiento).ToList();
            if (seguimientos.Count == 0)
                return 0;

            return seguimientos[0].NroSeguimiento;
        }

        private string ObtenerNombreCalificacion(int clasificacionId)
        {
            if (clasificacionId == 1)
            {
                return "Asintomático";
            }
            else if (clasificacionId == 2)
            {
                return "Sintomático Leve";
            }
            else if (clasificacionId == 3)
            {
                return "Sintomático Moderado";
            }
            else
            {
                return "";
            }
        }

        private string ObtenerNombreEstado(int estadoId)
        {
            if (estadoId == 1)
            {
                return "Cuarentena";
            }
            else if (estadoId == 2)
            {
                return "Hospitalizado";
            }
            else if (estadoId == 3)
            {
                return "Fallecido";
            }
            else if (estadoId == 4)
            {
                return "Aislamiento";
            }
            else
            {
                return "";
            }
        }

        //public JsonResult ActualizarGrillaListaTrabajadores(string joinCodigos)
        //{
        //    var lista = new List<ListaTrabajadoresViewModel>();
        //    if (joinCodigos == "")
        //        return Json(lista, JsonRequestBehavior.AllowGet);

        //    var arrCodigos = joinCodigos.Split(',');
        //    var arrint = Array.ConvertAll(arrCodigos, int.Parse).ToList();
        //    lista = ListaTrabajadores(arrint).ToList();

        //    return Json(lista, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult ActualizarGrillaListaTrabajadores(int Codigo)
        {
            var lista = ListaTrabajadores(Codigo).ToList().FindAll(p => p.EstadoClinicoId != 1);

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        #endregion

        private bool GetNroDiasSinSintomas(int registroTrabajadorId)
        {
            var seguimientos = db.Seguimiento.Where(w => w.RegistroTrabajadorId == registroTrabajadorId).ToList().OrderByDescending(p => p.NroSeguimiento).ToList();

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

            if (count >=7 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int GetNumeroDiasSinSintomas(int registroTrabajadorId)
        {
            var seguimientos = db.Seguimiento.Where(w => w.RegistroTrabajadorId == registroTrabajadorId).ToList().OrderByDescending(p => p.NroSeguimiento).ToList();

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

        private int GetNumeroDiasConSintomas(int registroTrabajadorId)
        {
            var seguimientos = db.Seguimiento.Where(w => w.RegistroTrabajadorId == registroTrabajadorId).ToList().OrderByDescending(p => p.NroSeguimiento).ToList();

            var count = 0;

            foreach (var item in seguimientos)
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
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        private int GetNumeroDiasSinSintomasAcumulado(int registroTrabajadorId)
        {
            var seguimientos = db.Seguimiento.Where(w => w.RegistroTrabajadorId == registroTrabajadorId).ToList().OrderByDescending(p => p.NroSeguimiento).ToList();

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
            }

            return count;
        }

        private int GetNumeroDiasConSintomasAcumulado(int registroTrabajadorId)
        {
            var seguimientos = db.Seguimiento.Where(w => w.RegistroTrabajadorId == registroTrabajadorId).ToList().OrderByDescending(p => p.NroSeguimiento).ToList();

            var count = 0;

            foreach (var item in seguimientos)
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
                    count++;
                }
            }

            return count;
        }

        public JsonResult IndicadoresPorEmpresa(int Codigo)
        {
            var oIndicadores = new Indicadores();

            var fechaActual = DateTime.Now.Date;

            var altas = (from A in db.RegistroTrabajador
                         join B in db.Seguimiento on A.Id equals B.RegistroTrabajadorId 
                         where A.EmpresaCodigo == Codigo && B.TipoEstadoId == (int)TipoEstado.AltaEpidemiologica
                         select B ).ToList();

            //var igms = (from A in db.RegistroTrabajador
            //             join B in db.Seguimiento on A.Id equals B.RegistroTrabajadorId
            //             where A.EmpresaCodigo == Codigo && B.ResultadoCovid19 == (int)ResultadoCovid19.IgMPositivo
            //             select B).ToList();

            //var iggs = (from A in db.RegistroTrabajador
            //            join B in db.Seguimiento on A.Id equals B.RegistroTrabajadorId
            //            where A.EmpresaCodigo == Codigo && B.ResultadoCovid19 == (int)ResultadoCovid19.IgGPositivo
            //            select B).ToList();

            //var igmiggs = (from A in db.RegistroTrabajador
            //            join B in db.Seguimiento on A.Id equals B.RegistroTrabajadorId
            //            where A.EmpresaCodigo == Codigo && B.ResultadoCovid19 == (int)ResultadoCovid19.IgMeIgGpositivo
            //            select B).ToList();

            var altasHoy = altas.Count(p => p.Fecha == fechaActual);
            var altasTotal = altas.Count;

            oIndicadores.altasHoy = altasHoy.ToString();
            oIndicadores.altasTotal = altasTotal.ToString();

            //oIndicadores.TotalIgM = igms.Count().ToString();
            //oIndicadores.TotalIgG = iggs.Count().ToString();
            //oIndicadores.TotalIgMeIgG = igmiggs.Count().ToString();

            return Json(oIndicadores, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAltas(int Codigo)
        {
            var lista = new List<InfoAltas>();
            var TrabajadoresAltas = (from A in db.RegistroTrabajador
                         where A.EmpresaCodigo == Codigo && A.EstadoClinicoId == (int)EstadoClinico.alta
                         select new InfoAltas {
                             TrabajadorId = A.Id,
                             Trabajador = A.NombreCompleto
                         }).ToList();


            foreach (var item in TrabajadoresAltas)
            {
                var oInfoAltas = new InfoAltas();
                var seguimientos = db.Seguimiento.Where(w => w.RegistroTrabajadorId == item.TrabajadorId).ToList().OrderByDescending(o => o.NroSeguimiento).ToList();
                oInfoAltas.Trabajador = item.Trabajador;
                oInfoAltas.NroSeguimientos = seguimientos[0].NroSeguimiento.ToString();
                oInfoAltas.NroSinSintomas = GetNumeroDiasSinSintomasAcumulado(item.TrabajadorId).ToString();
                oInfoAltas.NroConSintomas = GetNumeroDiasConSintomasAcumulado(item.TrabajadorId).ToString();
                lista.Add(oInfoAltas);
            }

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

       public class Indicadores
        {
            public string altasHoy { get; set; }
            public string altasTotal { get; set; }
            public string TotalIgM { get; set; }
            public string TotalIgG { get; set; }
            public string TotalIgMeIgG { get; set; }
        }

        public class InfoAltas
        {
            public int TrabajadorId { get; set; }
            public string Trabajador { get; set; }
            public string NroSeguimientos { get; set; }
            public string NroConSintomas { get; set; }
            public string NroSinSintomas { get; set; }
        }
    }
}