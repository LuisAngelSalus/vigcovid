using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigCovid.Common.BE
{
    [Table("Examen")]
   public class Examen
    {
        public int Id { get; set; }
        public int TrabajadorId { get; set; }
        public DateTime Fecha { get; set; }
        public int TipoPrueba { get; set; }
        public int Resultado { get; set; }
    }
}
