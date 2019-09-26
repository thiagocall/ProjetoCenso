using System.Collections.Generic;
using System.Threading.Tasks;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Parametros;

namespace Censo.API.Resultados
{
    public interface IOtimizacao
    {
         dynamic OtimizaCurso(Dictionary<long?, PrevisaoSKU> _dicPrevisao,
                              List<ProfessorCursoEmec> _professorEmec,
                              List<CursoProfessor> _listaProfessor,
                              List<CursoEnquadramento> _listaCursoEnquadramento);
        dynamic CalculaNotaCursos(List<ProfessorCursoEmec> _listaProfessorEmec,
                                         Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU,
                                         List<CursoProfessor> _listaCursoProfessor
                                            );
        double? N_Escala(double? lim_min, double? lim_max, double? percent);

    }




}