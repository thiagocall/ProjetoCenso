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
            });
        }
    }
}
