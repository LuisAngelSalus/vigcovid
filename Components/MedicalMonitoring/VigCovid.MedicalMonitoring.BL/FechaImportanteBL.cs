using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VigCovid.Common.AccessData;
using VigCovid.Common.BE;

namespace VigCovid.MedicalMonitoring.BL
{
   public class FechaImportanteBL
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool SaveFechasImportantes(List<FechaImportante> oFechaImportantes)         
        {
            try
            {
                var fechas = db.FechaImportante.Where(s => s.TrabajadorId == oFechaImportantes[0].TrabajadorId).ToList();
                db.FechaImportante.RemoveRange(fechas);

                #region Primera Fecha
                var oFechaImportante = new FechaImportante();

                oFechaImportante.TrabajadorId = oFechaImportantes[0].TrabajadorId;
                oFechaImportante.Descripcion = oFechaImportantes[0].Descripcion;
                oFechaImportante.Fecha = oFechaImportantes[0].Fecha;
                db.FechaImportante.Add(oFechaImportante);
                #endregion

                #region Segunda Fecha
                oFechaImportante = new FechaImportante();
                oFechaImportante.TrabajadorId = oFechaImportantes[0].TrabajadorId;
                oFechaImportante.Descripcion = oFechaImportantes[1].Descripcion;
                oFechaImportante.Fecha = oFechaImportantes[1].Fecha;
                db.FechaImportante.Add(oFechaImportante);
                #endregion

                #region Tercera Fecha
                oFechaImportante = new FechaImportante();
                oFechaImportante.TrabajadorId = oFechaImportantes[0].TrabajadorId;
                oFechaImportante.Descripcion = oFechaImportantes[2].Descripcion;
                oFechaImportante.Fecha = oFechaImportantes[2].Fecha;
                db.FechaImportante.Add(oFechaImportante);
                #endregion

                #region Cuarta Fecha
                oFechaImportante = new FechaImportante();
                oFechaImportante.TrabajadorId = oFechaImportantes[0].TrabajadorId;
                oFechaImportante.Descripcion = oFechaImportantes[3].Descripcion;
                oFechaImportante.Fecha = oFechaImportantes[3].Fecha;
                db.FechaImportante.Add(oFechaImportante);
                #endregion


                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveFechaImportante(FechaImportante oFechaImportante)
        {
            try
            {
                var fechas = db.FechaImportante.Where(s => s.TrabajadorId == oFechaImportante.TrabajadorId && s.Descripcion == oFechaImportante.Descripcion).ToList();
                db.FechaImportante.RemoveRange(fechas);

                var oEntity = new FechaImportante();

                oEntity.TrabajadorId = oFechaImportante.TrabajadorId;
                oEntity.Descripcion = oFechaImportante.Descripcion;
                oEntity.Fecha = oFechaImportante.Fecha;
                db.FechaImportante.Add(oEntity);

                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }           
        }

        public List<FechaImportante> GetAllFechasImportantes(int trabajadorId)
        {
            var fechas = (from a in db.FechaImportante where a.TrabajadorId == trabajadorId select a).ToList();
            return fechas;
        }
    }
}
