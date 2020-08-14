using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VigCovidApp.ViewModels
{
    public class EmpresaSedeViewModel
    {
        public int EmpresaId { get; set; }
        public string EmpresaNombre { get; set; }
        public List<EmpresaSede> Sedes { get; set; }
    }

    public class EmpresaSede
    {
        public int SedeId { get; set; }
        public string SedeNombre { get; set; }

    }
}