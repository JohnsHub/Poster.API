using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Poster.API.Models;
using Poster.API.Models.DTOs;

namespace Poster.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _users;
        private readonly SignInManager<AppUser> _signIn;
        private readonly IConfiguration _config;

        public AccountController(
            UserManager<AppUser> users,
            SignInManager<AppUser> signIn,
            IConfiguration config)
        {
            _users = users;
            _signIn = signIn;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new AppUser { UserName = dto.Username, Email = dto.Email };
            var res = await _users.CreateAsync(user, dto.Password);
            if (!res.Succeeded) return BadRequest(res.Errors);

            var response = new RegisterResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName
            };
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _users.FindByNameAsync(dto.Username);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var chk = await _signIn.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!chk.Succeeded)
                return Unauthorized("Invalid credentials");

            var jwtSection = _config.GetSection("Jwt");
            var keyBytes = Encoding.UTF8.GetBytes(jwtSection["Key"]!);
            var creds = new SigningCredentials(
                                 new SymmetricSecurityKey(keyBytes),
                                 SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
                },
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var response = new LoginResponseDto { Token = jwtToken };
            return Ok(response);
        }
    }
}
