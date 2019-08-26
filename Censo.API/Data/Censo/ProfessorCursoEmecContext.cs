using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Censo.API.Model.Censo
{
    public partial class ProfessorCursoEmecContext : DbContext
    {
        public ProfessorCursoEmecContext()
        {
        }

        public ProfessorCursoEmecContext(DbContextOptions<ProfessorCursoEmecContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProfessorCursoEmec> ProfessorCursoEmec { get; set; }

//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             if (!optionsBuilder.IsConfigured)
//             {
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                 optionsBuilder.UseSqlServer("Data Source=db-alteryx.database.windows.net;Database=db-alteryx;User Id=db-admin;Password=8v&Kmu8b;");
//             }
//         }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ProfessorCursoEmec>(entity =>
            {
                entity.HasKey(e => new { e.CodCampus, e.CodCurso, e.NumHabilitacao, e.CodEmec, e.CpfProfessor })
                    .HasName("PK__Rel_Prof__54CCBB569E73C793");

                entity.ToTable("Rel_Professor_Curso_Emec");

                entity.Property(e => e.CodCampus).HasColumnName("COD_CAMPUS");

                entity.Property(e => e.CodCurso).HasColumnName("COD_CURSO");

                entity.Property(e => e.NumHabilitacao).HasColumnName("NUM_HABILITACAO");

                entity.Property(e => e.CodEmec).HasColumnName("COD_EMEC");

                entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");

                entity.Property(e => e.CodIes).HasColumnName("COD_IES");

                entity.Property(e => e.NomCursoCenso)
                    .HasColumnName("NOM_CURSO_CENSO")
                    .HasMaxLength(255);
            });
        }
    }
}
