using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VigCovid.Common.BE;

namespace VigCovid.Common.AccessData
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=ApplicationDbContext")
        {
        }

        public virtual DbSet<RegistroTrabajador> RegistroTrabajador { get; set; }
        public virtual DbSet<Seguimiento> Seguimiento { get; set; }
        public virtual DbSet<Examen> Examen { get; set; }
        public virtual DbSet<FechaImportante> FechaImportante { get; set; }

    }
}
