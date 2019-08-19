using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Campus;
using Censo.API.Model;
using Censo.API.ForaDeSede;
using System.Net.Http;
using Censo.API.Resultados;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

//using Newtonsoft.Json.
namespace Censo.API.Controllers
{

    public class ProfessorTeste {

        public string Professor;
        public List<int> Habilitacao;

    }
    public class ParametroController: ControllerBase
    {
        [Route("api/Params")]

            [HttpPost]
            public object Post([FromBody] List<ProfessorTeste> prof)
            {
                return prof.Select(x => x.Professor).ToList();
            }

    }



}