using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VigCovidApp.Models;
using VigCovidApp.ViewModels;

namespace VigCovidApp.Controllers
{
    public class ExamenController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public JsonResult CrearExamen(RegistrarExamen registrarExamen) {

            try
            {
                var oExamen = new Examen();

                oExamen.Fecha = registrarExamen.FechaExamen;
                oExamen.TrabajadorId = registrarExamen.TrabajadorId;
                oExamen.TipoPrueba = registrarExamen.TipoExamen;
                oExamen.Resultado = registrarExamen.ResultadoExamen;

                db.Examen.Add(oExamen);
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            
        }

        public void ActualizarExamen() { 
        
        }

        public JsonResult ListarExamenesTrabajador(int trabajadorId)
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

            return Json(examenes, JsonRequestBehavior.AllowGet);
        }

        private string ObtenerResultado(int resultado)
        {
            if (resultado == 0)
            {
                return "Negativo";
            }else if(resultado == 1)
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
            }else
            {
                return "";
            }
        }
    }
}