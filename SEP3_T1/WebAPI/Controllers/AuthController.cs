using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiContract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, login.Phone.ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretLeyThatIsMinimum32CharactersLong"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


      var token = new JwtSecurityToken(
          issuer: "YourIssuer",
          audience: "YourAudience",
          claims: claims,
          expires: DateTime.Now.AddMinutes(30),
          signingCredentials: creds);
      
      return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
   
}
