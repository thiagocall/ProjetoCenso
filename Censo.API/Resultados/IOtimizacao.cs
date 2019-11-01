using System.Collections.Generic;
using System.Threading.Tasks;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Parametros;

namespace Censo.API.Resultados
{
    public interface IOtimizacao
    {
         List<Resultado> OtimizaCurso(Dictionary<long?, PrevisaoSKU> _dicPrevisao,
                              List<ProfessorCursoEmec> _professorEmec,
                              List<CursoProfessor> _listaProfessor,
                              List<CursoEnquadramento> _listaCursoEnquadramento,
                              ParametrosCenso _parametros);
        List<Resultado> CalculaNotaCursos(Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU, List<CursoProfessor> _listaCursoProfessor);

        double? N_Escala(double? lim_min, double? lim_max, double? percent);

        dynamic MontaResultadoFinal(List<Resultado> _resultado);

        double? CalculaNota(CursoProfessor _cursoProfessor, Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU, string _regime, string _titulacao, int _indMovimento);

        bool RemoveProfessor(List<CursoProfessor> _ListaCursoProfessor, CursoProfessor _cursoProfessor,
                            Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU, ProfessorEmec _prof, string _indNaoEnade = null);

        void AddProfessor(CursoProfessor _cursoProfessor);


    }


}