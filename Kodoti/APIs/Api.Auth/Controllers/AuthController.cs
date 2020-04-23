using Api.Auth.DTOs;
using Api.Auth.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Auth.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserCreateDto model)
        {
            var result = await _userManager.CreateAsync(new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            }, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }

        // POST auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest("Acceso denegado");
            }

            var response = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (response.Succeeded)
            {
                return Ok(
                    new { 
                        access_token = Token(user),
                        user_email = user.Email,
                        user_id = user.Id
                    }
                );
            }

            return BadRequest("Acceso denegado");
        }

        private string Token(ApplicationUser user)
        {
            var secretKey = _configuration.GetValue<string>("SecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(createdToken);
        }
    }
}