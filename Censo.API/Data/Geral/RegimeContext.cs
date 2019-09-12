using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model
{
    public partial class RegimeContext : DbContext
    {

        public RegimeContext(DbContextOptions<RegimeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProfessorRegime> ProfessorRegime { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ProfessorRegime>(entity =>
            {
                entity.HasKey(e => e.CpfProfessor)
                    .HasName("PK__Rel_Prof__D2550FC3BE2FA207");

                entity.ToTable("Rel_Professor_Regime");

                entity.Property(e => e.CpfProfessor)
                    .HasColumnName("CPF_PROFESSOR")
                    .HasMaxLength(255)
                    .ValueGeneratedNever();

                entity.Property(e => e.CargaTotal).HasColumnName("Carga_Total");

                entity.Property(e => e.QtdHorasDs).HasColumnName("QtdHoras_DS");

                entity.Property(e => e.QtdHorasFs).HasColumnName("QtdHoras_FS");

                entity.Property(e => e.Regime)
                    .IsRequired()
                    .HasMaxLength(255);
            });
        }
    }
}
