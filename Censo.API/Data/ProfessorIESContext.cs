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
                entity.HasKey(e => new { e.NumMatricula, e.CodInstituicao })
                    .HasName("PK__Rel_Prof__161071DEF6796A1F");

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
            });
        }
    }
}
