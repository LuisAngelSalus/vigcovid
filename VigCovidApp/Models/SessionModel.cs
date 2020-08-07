using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VigCovidApp.Models
{
    public class SessionModel
    {
        public string UserName { get; set; }

        public List<EmpresaAsignada> EmpresasAsignadas { get; set; }
    }
}