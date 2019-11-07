using Censo.API.Model.Censo;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Data.Censo
{
    public partial class CensoContext : DbContext
    {

        public CensoContext(DbContextOptions<CensoContext> options) : base(options) {}

        public virtual DbSet<CursoCenso> CursoCenso { get; set; }
        public virtual DbSet<ProfessorCursoCenso> ProfessorCursoCenso { get; set; }

        public virtual DbSet<ProfessorCursoCenso20p> ProfessorCursoCenso20p { get; set; }

        public virtual DbSet<ProfessorCursoEmec> ProfessorCursoEmec { get; set; }
        public virtual DbSet<ProfessorCursoEmec20p> ProfessorCursoEmec20p { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CursoCenso>(entity =>
            {
                entity.HasKey(e => new { e.CodCampus, e.CodCurso, e.NumHabilitacao });

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

                entity.Property(e => e.IndEnade)
                    .HasColumnName("IND_ENADE")
                    .HasMaxLength(255);
            });


            modelBuilder.Entity<ProfessorCursoCenso>(entity =>
            {
                entity.HasKey(e => new { e.CpfProfessor, e.CodCampus, e.CodCurso, e.NumHabilitacao });

                entity.ToTable("Rel_Professor_Curso_Censo");

                entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");

                entity.Property(e => e.CodCampus).HasColumnName("COD_CAMPUS");

                entity.Property(e => e.CodCurso).HasColumnName("COD_CURSO");

                entity.Property(e => e.NumHabilitacao).HasColumnName("NUM_HABILITACAO");

                entity.Property(e => e.CodIes).HasColumnName("COD_IES");
            });


             modelBuilder.Entity<ProfessorCursoCenso20p>(entity =>
            {
                entity.HasKey(e => new { e.CpfProfessor, e.CodCampus, e.CodCurso, e.NumHabilitacao });

                entity.ToTable("Rel_Professor_Curso_Censo_20p");

                entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");

                entity.Property(e => e.CodCampus).HasColumnName("COD_CAMPUS");

                entity.Property(e => e.CodCurso).HasColumnName("COD_CURSO");

                entity.Property(e => e.NumHabilitacao).HasColumnName("NUM_HABILITACAO");

                entity.Property(e => e.CodIes).HasColumnName("COD_IES");
            });
            
            
            
             modelBuilder.Entity<ProfessorCursoEmec>(entity =>
            {
                entity.HasKey(e => new { e.CodCampus, e.CodCurso, e.NumHabilitacao, e.CodEmec, e.CpfProfessor });

                entity.ToTable("Rel_Professor_Curso_Emec");

                entity.Property(e => e.CodCampus).HasColumnName("COD_CAMPUS");

                entity.Property(e => e.CodCurso).HasColumnName("COD_CURSO");

                entity.Property(e => e.NumHabilitacao).HasColumnName("NUM_HABILITACAO");

                entity.Property(e => e.CodEmec).HasColumnName("COD_EMEC");

                entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");

                entity.Property(e => e.CodIes).HasColumnName("COD_IES");

                entity.Property(e => e.NomCursoCenso)
                    .HasColumnName("NOM_CURSO_CENSO")
                    .HasMaxLength(255);
                
                entity.Property(e => e.IndAtivo)
                    .HasColumnName("IND_ATIVO")
                    .HasMaxLength(255);
                
                entity.Property(e => e.Titulacao)
                    .HasColumnName("TITULACAO")
                    .HasMaxLength(255);

                 entity.Property(e => e.Regime)
                    .HasColumnName("REGIME")
                    .HasMaxLength(255);
                
            });


        modelBuilder.Entity<ProfessorCursoEmec20p>(entity =>
            {
                entity.HasKey(e => new { e.CodCampus, e.CodCurso, e.NumHabilitacao, e.CodEmec, e.CpfProfessor });

                entity.ToTable("Rel_Professor_Curso_Emec_20p");

                entity.Property(e => e.CodCampus).HasColumnName("COD_CAMPUS");

                entity.Property(e => e.CodCurso).HasColumnName("COD_CURSO");

                entity.Property(e => e.NumHabilitacao).HasColumnName("NUM_HABILITACAO");

                entity.Property(e => e.CodEmec).HasColumnName("COD_EMEC");

                entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");

                entity.Property(e => e.CodIes).HasColumnName("COD_IES");

                entity.Property(e => e.NomCursoCenso)
                    .HasColumnName("NOM_CURSO_CENSO")
                    .HasMaxLength(255);
                
                entity.Property(e => e.IndAtivo)
                    .HasColumnName("IND_ATIVO")
                    .HasMaxLength(255);
                
                entity.Property(e => e.Titulacao)
                    .HasColumnName("TITULACAO")
                    .HasMaxLength(255);

                 entity.Property(e => e.Regime)
                    .HasColumnName("REGIME")
                    .HasMaxLength(255);
                

            });
        }
    }
}