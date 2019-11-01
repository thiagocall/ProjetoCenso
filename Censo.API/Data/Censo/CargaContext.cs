using Censo.API.Model.Censo;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Data.Censo
{
    public class CargaContext: DbContext
    {
        
        public CargaContext(DbContextOptions<CargaContext> options) : base(options) {}


        public virtual DbSet<CargaDS> CargaDS { get; set; }
        public virtual DbSet<CargaFS> CargaFS { get; set; }


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



        }

    }

}