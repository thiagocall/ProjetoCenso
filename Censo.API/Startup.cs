using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Data.Censo;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Model.dados;
using Censo.API.Resultados;
using Censo.API.Services.Redis.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Censo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            

            services.AddTransient<RedisService>();
            services.AddDbContext<UserContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "APi Censo",
                    Description = "Acesso aos Recursos da API Estratégia Acadêmica",
                    TermsOfService = new Uri("https://example.com/terms"),
                    // Contact = new OpenApiContact
                    // {
                    //     Name = "Shayne Boyer",
                    //     Email = string.Empty,
                    //     Url = new Uri("https://twitter.com/spboyer"),
                    // },
                    // License = new OpenApiLicense
                    // {
                    //     Name = "Use under LICX",
                    //     Url = new Uri("https://example.com/license"),
                    // }
                });
            });


            IdentityBuilder builder = services.AddIdentityCore<ApplicationUser>(options => 
            {
                options.Password.RequireDigit = false; 
                options.Password.RequireNonAlphanumeric = false; 
                options.Password.RequireLowercase = false; 
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            });


            // Injeta as dependecias na controller
             builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
             builder.AddEntityFrameworkStores<UserContext>();
             builder.AddRoleValidator<RoleValidator<Role>>();
             builder.AddRoleManager<RoleManager<Role>>();
             builder.AddSignInManager<SignInManager<ApplicationUser>>();


            // COnfigurando o JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                                .GetBytes(Configuration.GetSection("JWT:Key").Value)),
                        ValidateIssuer = false,
                         ValidateAudience = false     
                    };
            });
            // Adiciona politica para liberaçao dos usuários atr
            services.AddAuthorization(options => {
             options.AddPolicy("RequireMaster", policy => {
                policy.RequireClaim("Roles","Master");
               // policy.RequireClaim("Roles","User");
             }
                );
            options.AddPolicy("RequireN0", policy =>
                policy.RequireClaim("Roles","User")
                );
                  options.AddPolicy("RequireN1", policy =>
                policy.RequireClaim("Roles","Master","Reg", "Adm", "CSC")
                );
                options.AddPolicy("RequireN2", policy =>
                policy.RequireClaim("Roles","Master","Reg", "Adm")
                );
                options.AddPolicy("RequireN3", policy =>
                policy.RequireClaim("Roles","Master", "Adm")
                );
            }
              
            );

            //Adiciona contexto do Censo para Controller
            services.AddDbContext<ProfessorContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ProfessorIESContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<RegimeContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<CampusContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<CensoContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ProfessorContratoContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<dadosContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ProfessorMatriculaContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<CursoEnquadramentoContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<TempProducaoContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection2")));
            services.AddDbContext<CargaContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<RegionalSiaContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ExportacaoContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<EnadeContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ProfessorAddContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            //Adicionando a autentiação as rotas
            services.AddMvc(options => {

                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                
            })
            
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = 
                                Newtonsoft.Json.ReferenceLoopHandling.Ignore); // evita a redundancia de referencia nos json
         

            services.AddScoped<IOtimizacao, Otimizacao>();
            
            services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, RedisService redisService, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            redisService.Connect();
            redisService.upService();
            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Censo V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();

        }



    }
}
