using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VigCovid.Common.AccessData;
using VigCovid.Common.BE;

namespace VigCovid.Security
{
    public class AccessBL
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool ValidarUsuario (string usuario, string password)
        {
            var Usuario = (from A in db.Usuario where A.NombreUsuario == usuario && A.PasswordUsuario == password select A).FirstOrDefault();
            if (Usuario != null)
            {
                return true;
            }
            return false;
        }

        public List<EmpresaSede> ObtenerEmpresaSedesPorUsuario(int UsuarioId)
        {
            return null;
        }
    }
}
