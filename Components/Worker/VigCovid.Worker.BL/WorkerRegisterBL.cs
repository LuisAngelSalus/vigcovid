using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VigCovid.Common.AccessData;
using VigCovid.Common.BE;

namespace VigCovid.Worker.BL
{
    
    public class WorkerRegisterBL
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool WorkerRegister(RegistroTrabajador registrarTrabajador)
        {
            try
            {
                registrarTrabajador.FechaIngreso = DateTime.Now.Date;
                db.RegistroTrabajador.Add(registrarTrabajador);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}
