using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigCovid.Common.BE
{
    [Table("RegistroTrabajador")]
    public class RegistroTrabajador
    {
        public int Id { get; set; }
        public int ModoIngreso { get; set; }
        public int ViaIngreso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string NombreCompleto { get; set; }
        public string ApePaterno { get; set; }
        public string ApeMaterno { get; set; }
        public string Dni { get; set; }
        public int Edad { get; set; }
        public string PuestoTrabajo { get; set; }
        public string Celular { get; set; }
        public string TelfReferencia { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public int? EmpresaCodigo { get; set; }
        public string MedicoVigila { get; set; }
        public int? EstadoClinicoId { get; set; }

    }
}
