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
            
        }
    }
}
