using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VigCovidApp.Models
{
    public class FechaImportante
    {
        public int FechaImportanteId { get; set; }
        public int TrabajadorId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Fecha { get; set; }
    }
}