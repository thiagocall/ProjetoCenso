using System.Collections.Generic;
using Censo.API.Model;
using Censo.API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Censo.API.Resultados
{
    public static class Professores
    {

       
        public static List<Professor> ListaProfessorIES;


        public static DbSet<Professor> getProfessores( ProfessorContext _context){

            var results = _context.Professores;
            return results;
        }

           public static DbSet<ProfessorIes> getProfessoresIES(ProfessorIESContext _context){

                var results = _context.ProfessorIES;
                return results;
        }

      
        
        

    }
}