using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model.dados
{
    public partial class dadosContext : DbContext
    {
        public dadosContext()
        {
        }

        public dadosContext(DbContextOptions<dadosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CampusSia> CampusSia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CampusSia>(entity =>
            {
                entity.HasKey(e => e.CodCampus)
                    .HasName("PK__TbSia_Ca__17B528E5B56D8CD4");

                entity.ToTable("TbSia_Campus");

                entity.HasIndex(e => e.CodCampus)
                    .HasName("Campus_IDX1");

                entity.Property(e => e.CodCampus)
                    .HasColumnName("COD_CAMPUS")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.CepCampus)
                    .HasColumnName("CEP_CAMPUS")
                    .HasMaxLength(255);

                entity.Property(e => e.CodCampusSap)
                    .HasColumnName("COD_CAMPUS_SAP")
                    .HasMaxLength(255);

                entity.Property(e => e.CodMunicipio)
                    .HasColumnName("COD_MUNICIPIO")
                    .HasColumnType("decimal(6, 0)");

                entity.Property(e => e.EndCampus)
                    .HasColumnName("END_CAMPUS")
                    .HasMaxLength(255);

                entity.Property(e => e.NomCampus)
                    .HasColumnName("NOM_CAMPUS")
                    .HasMaxLength(255);

                entity.Property(e => e.TxtComplEndereco)
                    .HasColumnName("TXT_COMPL_ENDERECO")
                    .HasMaxLength(255);
            });
        }
    }
}
