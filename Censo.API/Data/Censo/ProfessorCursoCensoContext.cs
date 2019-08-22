using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model.Censo
{
    public partial class ProfessorCursoCensoContext : DbContext
    {
        public ProfessorCursoCensoContext()
        {
        }

        public ProfessorCursoCensoContext(DbContextOptions<ProfessorCursoCensoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProfessorCursoCenso> ProfessorCursoCenso { get; set; }

//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             if (!optionsBuilder.IsConfigured)
//             {
// //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                 optionsBuilder.UseSqlServer("Data Source=db-alteryx.database.windows.net;Database=db-alteryx;User Id=db-admin;Password=8v&Kmu8b;");
//             }
//         }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ProfessorCursoCenso>(entity =>
            {
                entity.HasKey(e => new { e.CpfProfessor, e.CodCampus, e.CodCurso, e.NumHabilitacao })
                    .HasName("PK__Rel_Prof__749E57416DB04B60");

                entity.ToTable("Rel_Professor_Curso_Censo");

                entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");

                entity.Property(e => e.CodCampus).HasColumnName("COD_CAMPUS");

                entity.Property(e => e.CodCurso).HasColumnName("COD_CURSO");

                entity.Property(e => e.NumHabilitacao).HasColumnName("NUM_HABILITACAO");

                entity.Property(e => e.CodIes).HasColumnName("COD_IES");
            });
        }
    }
}
