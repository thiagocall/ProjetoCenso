using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model
{
    public partial class CampusContext : DbContext
    {
        public CampusContext()
        {
        }

        public CampusContext(DbContextOptions<CampusContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Campus> TbSiaCampus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Campus>(entity =>
            {
                entity.HasKey(e => e.CodCampus);
                    

                entity.ToTable("TbSia_Campus");

                entity.HasIndex(e => e.CodCampus)
                    .HasName("Campus_IDX1");

                entity.Property(e => e.CodCampus)
                    .HasColumnName("COD_CAMPUS");

                entity.Property(e => e.CepCampus)
                    .HasColumnName("CEP_CAMPUS")
                    .HasMaxLength(255);

                entity.Property(e => e.CodCampusSap)
                    .HasColumnName("COD_CAMPUS_SAP")
                    .HasMaxLength(255);

                entity.Property(e => e.EndCampus)
                    .HasColumnName("END_CAMPUS")
                    .HasMaxLength(255);

                entity.Property(e => e.NomCampus)
                    .HasColumnName("NOM_CAMPUS")
                    .HasMaxLength(255);

                entity.Property(e => e.IndSituacao)
                    .HasColumnName("IND_SITUACAO");

                entity.Property(e => e.TxtComplEndereco)
                    .HasColumnName("TXT_COMPL_ENDERECO")
                    .HasMaxLength(255);
            });
        }
    }
}
