using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.ForaDeSede
{
    public static class ForaDeSedePr
    {

        public static List<ProfessorIes> OtimizaProfessorForaDeSede(DbSet<ProfessorIes> _professor, Dictionary<string, List<string>> _campusProfessor)
        {
            // ########### Método para criação dos professores fora de sede ############## //

            //GenericUriParser lista Fora de Sede
            List<string> listaForaSede = new List<string>(){
                    "4"
                    ,"5"
                    ,"7"
                    ,"8"
                    ,"33"
                    ,"42"
                    ,"43"
                    ,"44"
                    ,"49"
                    ,"51"
                    ,"52"
                    ,"61"
                    ,"67"
                    ,"297"
                    ,"301"
                    ,"564"
                    ,"720"
                    ,"721"
                    ,"1002"
            };

            // Inicia ajuste nos professores fora de Sede

            // Gera professores elegíveis ao FS

            //var professores = _campusProfessor.Where(p => p.Value.Any(x => listaForaSede.Any(y => x.Contains(y)))).Select(x => x).ToList();
            
            var professores = _campusProfessor.Where(p => p.Value.Any( c => listaForaSede.Contains(c))).ToDictionary(x => x.Key, x => x.Value);
            var professor_ies = _professor.Where(p => professores.ContainsKey(p.CpfProfessor.ToString())).ToList();

            //Verifica professor ofensor na sede

            //var query = professor_ies.Where(x => x.regime != "HORISTA" & x.titulacao != "ESPECIALISTA").ToList();

            return professor_ies;

            
        }





        
    }
}