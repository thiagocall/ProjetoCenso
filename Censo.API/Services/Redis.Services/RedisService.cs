using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Data.Censo;
using Censo.API.Model;
using Censo.API.Resultados;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace Censo.API.Services.Redis.Services
{
   public class RedisService
{
  private readonly string _redisHost;
  private readonly int _redisPort;

  IRedisClient _redis;
  private RegimeContext regContext;
        private ProfessorContext context;

        public RedisService(IConfiguration config,
                            ProfessorContext Context,
                            RegimeContext RegContext,
                            CensoContext  Censocontext, 
                            CampusContext campusContext,
                            ProfessorMatriculaContext _matContext)
        {
                // ctor da classe

            _redisHost = config["Redis:Host"];
            _redisPort = Convert.ToInt32(config["Redis:Port"]);
            this.regContext = RegContext;
            this.context = Context;
        
        }

    public void Connect() {

        try
      {
          var configString = $"{_redisHost}:{_redisPort}";
          RedisConfig.DefaultRetryTimeout = 2000;
          RedisConfig.DefaultSendTimeout = 2000;
          _redis = new RedisClient(configString);
      }
      catch (RedisException err)
      {

      }

    }

public IRedisClient GetRedisClient() {

    return this._redis;
}

public void upService () {


    try
    {
            using (var redis = this.GetRedisClient())
            {
                

                
                IRedisTypedClient<Resumo> res = redis.As<Resumo>() as IRedisTypedClient<Resumo>;
                
                if (res.GetById(2020) == null)
                {

                 redis.FlushDb();
                var tsk1 = Task.Run( () => setProfessores());

                Task.WaitAll(tsk1);

                var resumo = tsk1.Result;

                res.Store(resumo, new TimeSpan(10,0,0,0));
                res.Save();
                }

            }

            }
    catch (System.Exception ex)
    {
        
    }

}


public Resumo getProfessores() {

    Resumo resumo;

    try
    {
        using (var redis = this.GetRedisClient())
                {
                    
                    // redis.FlushDb();
                    IRedisTypedClient<Resumo> res = redis.As<Resumo>();
                    // redis.Info
                    
                    resumo = (Resumo)res.GetById(2020);
                } 

        return resumo;
        
    }
    catch (System.Exception ex )
    {
       return null;
    }


}


public async Task<Resumo> setProfessores() {

            Dictionary<string, ProfessorRegime> dic = new Dictionary<string, ProfessorRegime>();
            
                 // erro 100
                 Task task1 = Task.Factory.StartNew (
                    () => 
                    {
                      //dic = regContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());
                      dic = regContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor);
                    }
                    );

                    Task.WaitAll(task1);
                
        
                    var results =  await Professores.getProfessores(this.context).ToListAsync();

                        foreach (var item in results)
                        {
                            if (dic.ContainsKey(item.CpfProfessor.ToString()))
                            {
                                item.regime = dic[item.CpfProfessor.ToString()].Regime;
                            }

                            else
                            {
                                item.regime = "CHZ/AFASTADO";
                            }
                        }

                     
                    var qtdProfessores = results.Count();
                    var qtdDoutor = results.Where(x => x.Titulacao == "DOUTOR").Count();
                    var qtdMestre = results.Where(x => x.Titulacao == "MESTRE").Count();
                    var qtdEspecialista = results.Where(x => x.Titulacao == "ESPECIALISTA").Count();
                    var qtdNTitulado = results.Where(x => x.Titulacao == "NÃO IDENTIFICADA").Count();
                    var qtdRegime = results.Where(x => x.regime == "TEMPO INTEGRAL" | x.regime == "TEMPO PARCIAL" ).Count();
                    var qtdTempoIntegral = results.Where(x => x.regime == "TEMPO INTEGRAL").Count();
                    var qtdTempoParcial = results.Where(x => x.regime == "TEMPO PARCIAL" ).Count();
                    var qtdHorista = results.Where(x => x.regime == "HORISTA" ).Count();

                    var res = new Resumo { 
                                    Id = 2020,
                                    qtdDoutor = qtdDoutor, 
                                    qtdMestre = qtdMestre,
                                    qtdRegime = qtdRegime,
                                    qtdTempoIntegral = qtdTempoIntegral,
                                    qtdTempoParcial = qtdTempoParcial,
                                    qtdHorista = qtdHorista,
                                    qtdProfessores = qtdProfessores,
                                    qtdNTitulado = qtdNTitulado,
                                    qtdEspecialista = qtdEspecialista};

                  return res;
        }


}

public class Resumo 
    {

        public int Id { get; set; }
        public int qtdDoutor {get; set;}
        public int qtdMestre {get; set;}
        public int qtdRegime {get; set;}
        public int qtdTempoIntegral {get; set;}
        public int qtdTempoParcial {get; set;}
        public int qtdHorista {get; set;}
        public int qtdProfessores {get; set;}
        public int qtdNTitulado {get; set;}
        public int qtdEspecialista {get; set;}

    }

}