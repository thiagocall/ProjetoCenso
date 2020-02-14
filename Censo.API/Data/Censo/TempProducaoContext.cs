using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model.Censo
{
    public partial class TempProducaoContext : DbContext
    {
        public TempProducaoContext()
        {
        }

        public TempProducaoContext(DbContextOptions<TempProducaoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbResultado> TbResultado { get; set; }
        public virtual DbSet<TbResultadoAtual> TbResultadoAtual { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<TbResultado>(entity =>
            {
               // entity.HasKey("num_ordem");
                entity.ToTable("TbResultado");

                entity.Property(e => e.Id)
                    .HasColumnName("num_ordem")
                    .ValueGeneratedNever();

                entity.Property(e => e.Resultado)
                    .IsRequired()
                    .HasColumnName("resultado");

                entity.Property(e => e.Parametro)
                    .HasColumnName("parametro");

                entity.Property(e => e.Resumo)
                    .HasColumnName("resumo");

                entity.Property(e => e.Professores)
                    .HasColumnName("professores");

                entity.Property(e => e.TempoExecucao)
                    .HasColumnName("tempo_execucao");

                entity.Property(e => e.indOficial)
                    .HasColumnName("ind_oficial");
            });

            modelBuilder.Entity<TbResultadoAtual>(entity =>
            {
               // entity.HasKey("num_ordem");
                entity.ToTable("TbResultado_Atual");

                entity.Property(e => e.Id)
                    .HasColumnName("num_ordem")
                    .ValueGeneratedNever();

                entity.Property(e => e.Resultado)
                    .IsRequired()
                    .HasColumnName("resultado");

                entity.Property(e => e.Parametro)
                    .HasColumnName("parametro");

                entity.Property(e => e.Resumo)
                    .HasColumnName("resumo");

                entity.Property(e => e.Professores)
                    .HasColumnName("professores");
            });


        }
    }
}
