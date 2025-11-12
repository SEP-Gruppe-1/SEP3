using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    [HttpGet("{phone:int}")]
    public async Task<ActionResult<>> GetCustomer(int phone)
    {
        try
        {
            var customer = await customerRepository.GetSingleAsync(phone)
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}