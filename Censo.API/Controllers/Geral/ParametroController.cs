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
using Censo.API.Parametros;

//using Newtonsoft.Json.
namespace Censo.API.Controllers
{
    public class ParametroController: ControllerBase
    {
        [Route("api/Params")]

            [HttpPost]
            public object Post([FromBody] List<string> prof)
            {
                
                ParametrosFiltro.setListaProfessor(prof.ToList());
                return ParametrosFiltro.ListaProfessores.Count();

            }

    }



}