using Censo.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Censo.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public UsuariosController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

         [HttpGet]
         public ActionResult<string> Get()
         {
            return " << Controlador UsuariosController :: WebApiUsuarios >> ";
         }
        [AllowAnonymous]
        [HttpPost("Criar")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Created("BuildToken", model);
            }
            else
            {
                return BadRequest("Usuário ou senha inválidos");
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {

            try
            {
                    var user = await _userManager.FindByNameAsync(userInfo.Email);

                    var result = await _signInManager.CheckPasswordSignInAsync(user, userInfo.Password, 
                        false);

                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.NormalizedUserName == userInfo.Email.ToString().ToUpper());

                        return Ok(BuildToken(userInfo));
                        
                        // BuildToken(userInfo);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "login inválido.");
                        return Unauthorized();
                    }
                        
            }
            catch (System.Exception)
            {
                
                    return StatusCode (StatusCodes.Status500InternalServerError, "Erro na requisição");
            }

            


        }
        private UserToken BuildToken(UserInfo userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("meuValor", "oque voce quiser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.ASCII
                                .GetBytes(_configuration.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // tempo de expiração do token: 1 hora
            var expiration = DateTime.UtcNow.AddHours(24);
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);
            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}