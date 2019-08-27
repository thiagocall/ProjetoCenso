using System.Collections.Generic;

namespace Censo.API.Parametros
{
    public static class ParametrosFiltro
    {

        public static List<string> ListaProfessores;

        public static bool indGraduado;

        public static void setListaProfessor(List<string> _listaProfessores){
      
                ListaProfessores = _listaProfessores;
           
        }

        
    }
}