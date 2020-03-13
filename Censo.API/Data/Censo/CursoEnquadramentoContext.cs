using System;
using Censo.API.Model.Censo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model
{
    public partial class CursoEnquadramentoContext : DbContext
    {
        public CursoEnquadramentoContext()
        {
        }

        public CursoEnquadramentoContext(DbContextOptions<CursoEnquadramentoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CursoEnquadramento> CursoEnquadramento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CursoEnquadramento>(entity =>
            {
                entity.HasKey(e => e.CodEmec)
                .HasName("PK_cod_emec");
                                
                entity.ToTable("Rel_Curso_Enquadramento_Emec");

                entity.Property(e => e.CodEmec)
                .HasColumnName("cod_emec");

                entity.Property(e => e.CodArea)
                .HasColumnName("cod_area");
            });
        }
    }
}
