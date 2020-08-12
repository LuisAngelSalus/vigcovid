namespace VigCovidApp
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using VigCovidApp.Models;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegistroTrabajador>()
                .Property(e => e.NombreCompleto)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroTrabajador>()
                .Property(e => e.Dni)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroTrabajador>()
                .Property(e => e.PuestoTrabajo)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroTrabajador>()
                .Property(e => e.Celular)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroTrabajador>()
                .Property(e => e.TelfReferencia)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroTrabajador>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroTrabajador>()
                .Property(e => e.Direccion)
                .IsUnicode(false);
        }
    }
}
