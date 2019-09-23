using System;
using Censo.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Data
{
    public partial class ProfessorIESContext : DbContext
    {

        public ProfessorIESContext(DbContextOptions<ProfessorIESContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProfessorIes> ProfessorIES { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ProfessorIes>(entity =>
            {
                entity.HasKey(e => new {e.CpfProfessor, e.NumMatricula, e.CodInstituicao })
                    .HasName("PK__Rel_Prof__233408DE191E2C2C");

                entity.ToTable("Rel_Professor_IES");

                entity.Property(e => e.NumMatricula)
                    .HasColumnName("NUM_MATRICULA")
                    .HasMaxLength(255);

                entity.Property(e => e.CodInstituicao).HasColumnName("COD_INSTITUICAO");

                entity.Property(e => e.CodProfessor)
                    .HasColumnName("COD_PROFESSOR")
                    .HasMaxLength(255);

                entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");

                entity.Property(e => e.NomInstituicao)
                    .HasColumnName("NOM_INSTITUICAO")
                    .HasMaxLength(255);

                entity.Property(e => e.NomRegiao)
                    .HasColumnName("NOM_REGIAO")
                    .HasMaxLength(255);

                entity.Property(e => e.NomProfessor)
                    .HasColumnName("NOM_PROFESSOR")
                    .HasMaxLength(255);

                entity.Property(e => e.titulacao)
                    .HasColumnName("TITULACAO")
                    .HasMaxLength(255);

                 entity.Property(e => e.ativo)
                    .HasColumnName("ATIVO")
                    .HasMaxLength(255);
            });
        }
    }
}
