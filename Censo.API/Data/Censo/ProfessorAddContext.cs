using Censo.API.Model;
using Censo.API.Model.Censo;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Data.Censo
{
    public class ProfessorAddContext: DbContext
    {
        
        public ProfessorAddContext(DbContextOptions<ProfessorAddContext> options) : base(options) {}


        public virtual DbSet<ProfessorAdd> Professor { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ProfessorAdd>(entity =>
                    {
                        entity.HasKey(e => e.NumSeq)
                            .HasName("K_prof_curso");

                        entity.ToTable("Rel_Ajuste_Professor_Curso");

                        entity.Property(e => e.NumSeq).HasColumnName("NUM_SEQ_PROFESSOR_CURSO");
                        entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");
                        entity.Property(e => e.CodEmec).HasColumnName("EMEC");
                        entity.Property(e => e.Motivo).HasColumnName("MOTIVO");

                    }
            
            );

        }

    }

}