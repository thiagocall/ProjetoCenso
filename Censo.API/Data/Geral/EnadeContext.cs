using System;
using Censo.API.Model.Censo;
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

        public virtual DbSet<Enquadramento> Enquadramento { get; set; }

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

                entity.Property(e => e.AnoAtual)
                    .HasColumnName("ANO_ATUAL"); 
                           
                entity.Property(e => e.AnoAnterior)
                    .HasColumnName("ANO_ANTERIOR"); 

            });

            modelBuilder.Entity<EmecCiclo>(entity =>
            {
                entity.ToTable("Rel_Emec_Ciclo");
                
               entity.HasKey(e => e.CodAreaEmec)
                     .HasName("PK_COD_AREA_EMEC");

                entity.Property(e => e.CodAreaEmec)
                    .HasColumnName("COD_AREA_EMEC");

                entity.Property(e => e.IdCiclo)
                    .HasColumnName("ID_CICLO");

            });

           modelBuilder.Entity<Enquadramento>(entity =>
            {
                entity.ToTable("Rel_Enquadramento_emec");
                
                entity.HasKey(e => e.CodEnq)
                     .HasName("PK_Rel_enquadramento_emec");

                entity.Property(e => e.CodEnq)
                    .HasColumnName("COD_ENQUADRAMENTO");

                entity.Property(e => e.NomEnq)
                    .HasColumnName("NOM_ENQUADRAMENTO");
               
            });


        }
    }
}
