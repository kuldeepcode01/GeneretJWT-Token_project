using JWT_Token.Bl.InterFace;
using JWT_Token.Db.AppDbConnection;
using JWT_Token.Db.Models;
using JWT_Token.Db.Models.Api;
using JWT_Token.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GeneretJWT_Token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration,AppDbContext context)
        {
          
            _configuration = configuration;
            _context = context;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel dto)
        {
            var chekUser = await _context.UserTable.FirstOrDefaultAsync(x => x.Email == dto.UserName && x.Password == dto.Password);
            if (chekUser == null)
            {
                return Unauthorized();
            }
            var token = GeneretJwtToken(chekUser);
            return Ok(new { token });

        }
        
        [HttpGet("protected")]
        [Authorize]
        public IActionResult GetProtectedDat()
        {
            return Ok("This is a Protected endpoint");
        }
        private string GeneretJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim(ClaimTypes.Role,"User")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
            };
            var token=tokenHandler.CreateToken(tokenDescriptor);    
            return tokenHandler.WriteToken(token);
        }
        



            
    }
}
