using Microsoft.EntityFrameworkCore;

namespace Censo.API.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){}

        public DbSet<Professor> Professores {get; set;} 
    }
}