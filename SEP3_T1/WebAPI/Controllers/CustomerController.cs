using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using ApiContract;
using gRPCRepositories;
using Microsoft.AspNetCore.Http.HttpResults;

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

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> AddCustomer([FromBody] CustomerCreateDto request)
    {
        await customerRepository.VerifyCustomerDoesNotExist(request.Phone, request.Email);
        Customer customer = new()
        {
            Phone = request.Phone,
            Email = request.Email,
            Password = request.Password,
            Name = request.Name
        };
        await customerRepository.SaveCustomer(customer);
        SaveCustomerDto dto = new(
            request.Name,
            request.Phone,
            request.Email,
            request.Password
        );
        return Created($"/api/customer/{dto.Phone}", dto);
    }
}