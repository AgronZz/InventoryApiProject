

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InventoryApiProject.Auth;
using InventoryApiProject.Dtos;

namespace InventoryApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // =====================================
        // LOGIN
        // =====================================
        [HttpPost]
        public IActionResult Login([FromBody] UserDto userDto)
        {
            //Checks username/password from the table Users
            var user = UserConstants.Users.FirstOrDefault(x =>
                x.Username == userDto.Username &&
                x.Password == userDto.Password);

            if (user == null)  //if user is not found, return 401 Unauthorized
            {
                _logger.LogWarning("Invalid login attempt");

                return Unauthorized("Invalid username or password");
            }

            var token = CreateToken(user);  //if user is found, create JWT token

            _logger.LogInformation("User logged in: {Username}", user.Username);

            return Ok(new
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        }

        // =====================================
        // CREATE JWT TOKEN
        // =====================================
        private string CreateToken(UserModel user)
        {
            //create secret key
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:IssuerSigningKey"]));

            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            //Claims are information stored inside JWT
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),  //token valid for 3 hours
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}