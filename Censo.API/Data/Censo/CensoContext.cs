using Censo.API.Model.Censo;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Data.Censo
{
    public partial class CensoContext : DbContext
    {

        public CensoContext(DbContextOptions<CensoContext> options) : base(options) {}


        public virtual DbSet<CursoCenso> CursoCenso { get; set; }
        public virtual DbSet<ProfessorCursoCenso> ProfessorCursoCenso { get; set; }
        public virtual DbSet<ProfessorCursoEmec> ProfessorCursoEmec { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CursoCenso>(entity =>
            {
                entity.HasKey(e => new { e.CodCampus, e.CodCurso, e.NumHabilitacao })
                    .HasName("PK__Tb_Base___6CB5882B8B1659D7");

                entity.ToTable("Tb_Base_Curso_Censo");

                entity.Property(e => e.CodCampus).HasColumnName("COD_CAMPUS");

                entity.Property(e => e.CodCurso).HasColumnName("COD_CURSO");

                entity.Property(e => e.NumHabilitacao)
                    .HasColumnName("NUM_HABILITACAO")
                    .HasMaxLength(255);

                entity.Property(e => e.CodEmec).HasColumnName("COD_EMEC");

                entity.Property(e => e.CodIes).HasColumnName("COD_IES");

                entity.Property(e => e.CodArea).HasColumnName("COD_AREA");

                entity.Property(e => e.NomCursoCenso)
                    .HasColumnName("NOM_CURSO_CENSO")
                    .HasMaxLength(255);
            });
        }
    }
}