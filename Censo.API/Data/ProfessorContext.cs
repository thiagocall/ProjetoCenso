using System;
using Censo.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Data
{
    public partial class ProfessorContext : DbContext
    {

        public ProfessorContext(DbContextOptions<ProfessorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Professor> Professores { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.CpfProfessor)
                    .HasName("PK__Rel_Base__D2550FC3C26C187F");

                entity.ToTable("Rel_Base_Docente_Censo");

                entity.Property(e => e.CpfProfessor)
                    .HasColumnName("CPF_PROFESSOR")
                    .HasMaxLength(11);

                entity.Property(e => e.Ativo)
                    .HasColumnName("ATIVO_31_12")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CodSexo)
                    .HasColumnName("COD_SEXO")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Def1)
                    .HasColumnName("DEF1")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Def2)
                    .HasColumnName("DEF2")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Def3)
                    .HasColumnName("DEF3")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DocenteDeficiencia)
                    .HasColumnName("DOCENTE_DEFICIENCIA")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DocenteSubstituto)
                    .HasColumnName("DOCENTE_SUBSTITUTO")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DtNascimentoProfessor)
                    .HasColumnName("DT_NASCIMENTO_PROFESSOR")
                    .HasColumnType("datetime");

                entity.Property(e => e.Escolaridade)
                    .HasColumnName("ESCOLARIDADE")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MunicipioNascimento)
                    .HasColumnName("MUNICIPIO_NASCIMENTO")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NacionalidadeProfessor)
                    .HasColumnName("NACIONALIDADE_PROFESSOR")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NomMae)
                    .HasColumnName("NOM_MAE")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NomPais)
                    .HasColumnName("NOM_PAIS")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NomProfessor)
                    .HasColumnName("NOM_PROFESSOR")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NomRaca)
                    .HasColumnName("NOM_RACA")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Perfil)
                    .HasColumnName("PERFIL")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Pesquisa)
                    .HasColumnName("PESQUISA")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Titulacao)
                    .HasColumnName("TITULACAO")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UfNascimento)
                    .HasColumnName("UF_NASCIMENTO")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
        }
    }
}
