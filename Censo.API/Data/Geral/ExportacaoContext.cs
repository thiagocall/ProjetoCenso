using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model.dados
{
    public partial class ExportacaoContext : DbContext
    {


        public ExportacaoContext(DbContextOptions<ExportacaoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProfessorAdicionado> ProfessorAdicionado { get; set; }

        public virtual DbSet<CursoEmecIes> CursoEmecIes { get; set; }

        public virtual DbSet<IesSiaEmec> IesSiaEmec { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");
            // Entidades e tabelas

            modelBuilder.Entity<ProfessorAdicionado>(entity =>
            {
                entity.ToTable("TbAux_Professor_Adicionado");
                
                entity.HasKey(e => e.CpfProfessor)
                     .HasName("PK_CPF_PROFESSOR");

                entity.Property(e => e.CpfProfessor)
                    .HasColumnName("CPF_PROFESSOR");

                entity.Property(e => e.Regime)
                    .HasColumnName("Regime");

                entity.Property(e => e.Titulacao)
                    .HasColumnName("Titulacao");    

                entity.Property(e => e.qtdHorasDs)
                    .HasColumnName("qtdHorasDs");        
                
                entity.Property(e => e.qtdHorasFs)
                    .HasColumnName("qtdHorasFs");        

            });

            modelBuilder.Entity<CursoEmecIes>(entity =>
            {
                entity.ToTable("Rel_Curso_Emec_IES");
                
               entity.HasKey(e => e.CodCursoEmec)
                     .HasName("PK_COD_CURSO_EMEC");

                entity.Property(e => e.CodCursoEmec)
                    .HasColumnName("COD_CURSO_EMEC");

                entity.Property(e => e.CodCurso)
                    .HasColumnName("COD_CURSO");

                entity.Property(e => e.CodCampus)
                    .HasColumnName("COD_CAMPUS");

                entity.Property(e => e.NumHabilitacao)
                    .HasColumnName("NUM_HABILITACAO");    

                entity.Property(e => e.Regional)
                    .HasColumnName("REGIONAL");        
                
                entity.Property(e => e.CodIes)
                    .HasColumnName("COD_IES");        

                entity.Property(e => e.NomIes)
                    .HasColumnName("NOM_IES");        

            });
       
            modelBuilder.Entity<IesSiaEmec>(entity =>
            {
                entity.ToTable("Rel_IES_SIA_EMEC");
                
               entity.HasKey(e => e.Cod_Ies)
                     .HasName("PK_COD_IES");
                
                entity.Property(e => e.Cod_Ies)
                    .HasColumnName("COD_IES");        

                entity.Property(e => e.Cod_Ies_Emec)
                    .HasColumnName("COD_IES_EMEC");  

                entity.Property(e => e.Nom_Ies)
                    .HasColumnName("NOM_IES");        

            });

        }
    }
}
