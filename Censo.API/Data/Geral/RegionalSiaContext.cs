using System;
using Censo.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model
{
    public class RegionalSiaContext : DbContext
    {
        public RegionalSiaContext(DbContextOptions<RegionalSiaContext> options) : base(options)
        {

        }

        public virtual DbSet<RegionalSia> RegionalSia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<RegionalSia>(entity =>
                    {
                        entity.HasKey(e => e.CodCampus);

                        entity.ToTable("TbSia_Regional_SIA");

                        entity.Property(e => e.CodCampus).HasColumnName("COD_CAMPUS");
                        entity.Property(e => e.NomCampus).HasColumnName("CAMPUS");
                        entity.Property(e => e.Regional).HasColumnName("REGIONAL");
                        entity.Property(e => e.CodIes).HasColumnName("COD_IES");
                        entity.Property(e => e.NomIes).HasColumnName("NOM_IES");
                        entity.Property(e => e.CodCampusPai).HasColumnName("CAMPUS_PAI");

                    }
            );
            }


    }
}
