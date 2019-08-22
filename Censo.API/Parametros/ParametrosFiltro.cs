using System.Collections.Generic;

namespace Censo.API.Parametros
{
    public static class ParametrosFiltro
    {

        public static List<string> ListaProfessores;

        public static void setListaProfessor(List<string> _listaProfessores){

            if (ListaProfessores == null){

                ListaProfessores = _listaProfessores;
            }

        }



        
    }
}