using Censo.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Censo.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // Remove Endpoints do Swagger
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<Role> _roleManager ;
        public UsuariosController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

         [HttpGet]
         private ActionResult<string> Get()
         {
            return " << Controlador UsuariosController :: WebApiUsuarios >> ";
         }

        [AllowAnonymous]
        [HttpPost("Criar")]
        internal async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user,"User");
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

                        return Ok(await BuildToken(userInfo, user));
                        
                        // BuildToken(userInfo);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "login inválido.");
                        return Unauthorized();
                    }
                        
            }
            catch (System.Exception ex )
            {
                
                    return StatusCode (StatusCodes.Status500InternalServerError, "Erro na requisição");
            }

        
        }

          private async Task<UserToken> BuildToken(UserInfo userInfo, ApplicationUser _user) 
          {
            var user = _user;
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("Roles", userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            var key = new SymmetricSecurityKey(Encoding.ASCII
                                .GetBytes(_configuration.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // tempo de expiração do token: 1 dia
            var expiration = DateTime.UtcNow.AddDays(1);
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


        [Authorize(Policy=("RequireMaster"))]
        [HttpDelete("delUsuarios/{name}")]
        public async Task<IActionResult> delUsuarios(string name) {
            var user =await _userManager.FindByNameAsync(name);
            var delUser = await _userManager.DeleteAsync(user);
            if(delUser.Succeeded){
                return StatusCode(StatusCodes.Status200OK);
            }
            else {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [Authorize(Policy=("RequireMaster"))]
        [HttpDelete("delRole/{_name}/{_role}")]
        public async Task<IActionResult> delRole(string _name, string _role) {
            var user =await _userManager.FindByNameAsync(_name);
            var delRoler = await _userManager.RemoveFromRoleAsync(user, _role);
            if(delRoler.Succeeded){
                return StatusCode(StatusCodes.Status200OK);
            }
            else {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [Authorize(Policy=("RequireMaster"))]
        [HttpDelete("addRole/{_name}/{_role}")]
        public async Task<IActionResult> AddRole(string _name, string _role) {
            var user =await _userManager.FindByNameAsync(_name);
            var addRoler = await _userManager.AddToRoleAsync(user, _role);
            if(addRoler.Succeeded){
                return StatusCode(StatusCodes.Status200OK);
            }
            else {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
       

       [Authorize(Policy=("RequireMaster"))]
        [HttpGet("getUsuarios")]
        public async Task<IActionResult> getUsuarios() {
            var users = await _userManager.Users.ToListAsync();
            List<UserInfo> usersInfo = new List<UserInfo>();
            UserInfo userInfo;
            var activeuser = User.Identity.Name;

                foreach (var item in users)
                {
                    userInfo = new UserInfo(){
                        Email = item.UserName,
                        Roles = await _userManager.GetRolesAsync(item)
                    };

                    usersInfo.Add(userInfo);
                }

                var roles = _roleManager.Roles.ToList();

                var result = new {usersInfo, roles};

                return Ok(result);
    }
        



    }
}