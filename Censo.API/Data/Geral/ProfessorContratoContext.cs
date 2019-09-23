using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model
{
    public partial class ProfessorContratoContext : DbContext
    {
        public ProfessorContratoContext()
        {
        }

        public ProfessorContratoContext(DbContextOptions<ProfessorContratoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProfessorContrato> ProfessorContrato { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ProfessorContrato>(entity =>
            {
                entity.HasKey(e => new { e.CpfProfessor, e.NumMatricula })
                    .HasName("PK__Rel_Prof__BEBB4C4DA19FCFC4");

                entity.ToTable("Rel_Professor_Contrato");

                entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");

                entity.Property(e => e.NumMatricula)
                    .HasColumnName("NUM_MATRICULA")
                    .HasMaxLength(255);

                entity.Property(e => e.NomInstituicao)
                    .HasColumnName("NOM_INSTITUICAO")
                    .HasMaxLength(255);

                entity.Property(e => e.NomProfessor)
                    .HasColumnName("NOM_PROFESSOR")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NomRegiao)
                    .HasColumnName("NOM_REGIAO")
                    .HasMaxLength(255);

                entity.Property(e => e.SglUf)
                    .HasColumnName("SGL_UF")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
        }
    }
}
