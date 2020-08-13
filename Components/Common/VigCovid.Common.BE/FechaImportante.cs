using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigCovid.Common.BE
{
    [Table("FechaImportante")]
    public class FechaImportante
    {
        public int FechaImportanteId { get; set; }
        public int TrabajadorId { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
