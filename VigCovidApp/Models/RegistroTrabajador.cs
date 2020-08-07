namespace VigCovidApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RegistroTrabajador")]
    public partial class RegistroTrabajador
    {
        public int Id { get; set; }

        public int ModoIngreso { get; set; }
        public int ViaIngreso { get; set; }

        public DateTime FechaIngreso { get; set; }

        [StringLength(200)]
        public string NombreCompleto { get; set; }
        [StringLength(100)]
        public string ApePaterno { get; set; }
        [StringLength(100)]
        public string ApeMaterno { get; set; }

        [StringLength(20)]
        public string Dni { get; set; }

        public int? Edad { get; set; }

        [StringLength(200)]
        public string PuestoTrabajo { get; set; }

        [StringLength(50)]
        public string Celular { get; set; }

        [StringLength(50)]
        public string TelfReferencia { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }
        
        public int EmpresaCodigo { get; set; }

        public List<Seguimiento> Seguimientos { get; set; }

        public int? EstadoClinicoId { get; set; }
    }
}
