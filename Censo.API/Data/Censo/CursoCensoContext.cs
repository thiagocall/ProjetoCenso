using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model.Censo
{
    public partial class CursoCensoContext : DbContext
    {
        public CursoCensoContext()
        {
        }

        public CursoCensoContext(DbContextOptions<CursoCensoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CursoCenso> CursoCenso { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=db-alteryx.database.windows.net;Database=db-alteryx;User Id=db-admin;Password=8v&Kmu8b;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CursoCenso>(entity =>
            {
                entity.HasKey(e => new { e.CodCampus, e.CodCurso, e.NumHabilitacao })
                    .HasName("PK__Tb_Base___6CB5882B07EB82DD");

                entity.ToTable("Tb_Base_Curso_Censo");

                entity.Property(e => e.CodCampus).HasColumnName("COD_CAMPUS");

                entity.Property(e => e.CodCurso).HasColumnName("COD_CURSO");

                entity.Property(e => e.NumHabilitacao)
                    .HasColumnName("NUM_HABILITACAO")
                    .HasMaxLength(255);

                entity.Property(e => e.CodEmec).HasColumnName("COD_EMEC");

                entity.Property(e => e.CodIes).HasColumnName("COD_IES");

                entity.Property(e => e.NomCursoCenso)
                    .HasColumnName("NOM_CURSO_CENSO")
                    .HasMaxLength(255);
            });
        }
    }
}
