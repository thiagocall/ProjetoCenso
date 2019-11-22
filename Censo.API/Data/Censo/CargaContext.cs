using Censo.API.Model;
using Censo.API.Model.Censo;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Data.Censo
{
    public class CargaContext: DbContext
    {
        
        public CargaContext(DbContextOptions<CargaContext> options) : base(options) {}


        public virtual DbSet<CargaDS> CargaDS { get; set; }
        public virtual DbSet<CargaFS> CargaFS { get; set; }

        public virtual DbSet<ProfessorRegime> ProfessorRegime { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CargaDS>(entity =>
                    {
                        entity.HasKey(e => new { e.CpfProfessor });

                        entity.ToTable("Rel_MATRICULA_CARGA_DS");

                        entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");

                        entity.Property(e => e.QtdHoras).HasColumnName("Qtd_Horas");

                    }
            
            );


            modelBuilder.Entity<CargaFS>(entity =>

                {
                    entity.HasKey(e => new { e.CpfProfessor });

                    entity.ToTable("Rel_MATRICULA_CARGA_EX_DS");

                    entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");

                    entity.Property(e => e.QtdHoras).HasColumnName("Qtd_Horas");

                }


            );

            modelBuilder.Entity<ProfessorRegime>(entity =>
                    {
                        entity.HasKey(e => new { e.CpfProfessor });

                        entity.ToTable("Rel_Titulacao_Docente_Censo");

                        entity.Property(e => e.CpfProfessor).HasColumnName("CPF_PROFESSOR");

                        entity.Property(e => e.QtdHorasDs).HasColumnName("Qtd_Horas_DS");
                        
                        entity.Property(e => e.QtdHorasFs).HasColumnName("Qtd_Horas_FS");

                        entity.Property(e => e.CargaTotal).HasColumnName("Carga_Total");

                        entity.Property(e => e.Regime).HasColumnName("Regime");
                    }
            
            );

        }

    }

}