using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiContract;
using gRPCRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ICustomerRepository ICustomerRepository;

    public AuthController(ICustomerRepository customerRepository)
    {
        ICustomerRepository = customerRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        if (ICustomerRepository is CustomerInDatabaseRepository repo)
            await repo.InitializeAsync();

        var customer = await ICustomerRepository
            .GetByPhoneAndPasswordAsync(login.Phone, login.Password);

        if (customer == null)
            return Unauthorized("Invalid phone or password");

        // JWT generation …


        var claims = new[]
        {
            new Claim(ClaimTypes.Name, customer.Name),
            new Claim("phone", customer.Phone)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretThatIsMinimum32CharactersLong"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var token = new JwtSecurityToken(
            "YourIssuer",
            "YourAudience",
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}