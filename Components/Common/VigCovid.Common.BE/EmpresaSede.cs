using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigCovid.Common.BE
{
   public class EmpresaSede
    {
        public int EmpresaId { get; set; }
        public string EmpresaNombre { get; set; }
        public List<Sede> Sedes { get; set; }
    }
    public class Sede
    {
        public int SedeId { get; set; }
        public string SedeNombre { get; set; }

    }
}
