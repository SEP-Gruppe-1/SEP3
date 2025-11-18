using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using ApiContract;
using gRPCRepositories;

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
    public async Task<ActionResult<CustomerDto>> GetSingle(int phone)
    {
        try
        {
            var customer = await customerRepository.GetSingleAsync(phone);
            return Ok(new CustomerDto(customer.Name, customer.Phone, customer.Email));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
    
   
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        if (customerRepository is CustomerInDatabaseRepository repo)
        {
            await repo.InitializeAsync();
        }

        var customers = customerRepository.GetAll().ToList();
        return Ok(customers);
    }
}