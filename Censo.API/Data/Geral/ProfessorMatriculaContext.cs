using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model
{
    public partial class ProfessorMatriculaContext : DbContext
    {
        public ProfessorMatriculaContext()
        {
        }

        public ProfessorMatriculaContext(DbContextOptions<ProfessorMatriculaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProfessorMatricula> ProfessorMatricula { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ProfessorMatricula>(entity =>
            {
                entity.HasKey(e => e.numMatricula);

                entity.ToTable("Rel_PROFESSOR_MATRICULA");

                entity.HasIndex(e => e.numMatricula);

                entity.Property(e => e.numMatricula)
                    .HasColumnName("NUM_MATRICULA")
                    .HasMaxLength(255);

                entity.Property(e => e.cpfProfessor)
                    .HasColumnName("CPF_PROFESSOR");

                entity.Property(e => e.indTipoContrato)
                    .HasColumnName("IND_TIPO_CONTRATO")
                    .HasMaxLength(255);

                entity.Property(e => e.dtAdmissao)
                    .HasColumnName("DT_ADMISSAO_PROFESSOR");

                entity.Property(e => e.codRegiao)
                    .HasColumnName("COD_REGIAO")
                    .HasMaxLength(255);

                entity.Property(e => e.nomeRegiao)
                    .HasColumnName("NOM_REGIAO")
                    .HasMaxLength(255);
            }
            );
        }
    }
}
