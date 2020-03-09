using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model.dados
{
    public partial class EnadeContext : DbContext
    {


        public EnadeContext(DbContextOptions<EnadeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ciclo> Ciclo { get; set; }

        public virtual DbSet<EmecCiclo> EmecCiclo { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");
            // Entidades e tabelas

            modelBuilder.Entity<Ciclo>(entity =>
            {
                entity.ToTable("Rel_Ciclo");
                
                entity.HasKey(e => e.IdCiclo)
                     .HasName("PK_ID_CICLO");

                entity.Property(e => e.IdCiclo)
                    .HasColumnName("ID_CICLO");

                entity.Property(e => e.DescricaoCiclo)
                    .HasColumnName("DESC_CICLO");

                entity.Property(e => e.DescArea)
                    .HasColumnName("DESC_AREA");    

                entity.Property(e => e.Obs)
                    .HasColumnName("OBS");        

            });

            modelBuilder.Entity<EmecCiclo>(entity =>
            {
                entity.ToTable("Rel_Enade_Ciclo");
                
               entity.HasKey(e => e.CodCursoEmec)
                     .HasName("PK_COD_CURSO_EMEC");

                entity.Property(e => e.CodCursoEmec)
                    .HasColumnName("COD_CURSO_EMEC");

                entity.Property(e => e.IdCiclo)
                    .HasColumnName("ID_CICLO");

            });
      


        }
    }
}
