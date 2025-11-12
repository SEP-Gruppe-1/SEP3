using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using ApiContract;

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
    public ActionResult<IEnumerable<CustomerDto>> GetAll([FromQuery] string? name, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        {
            var query = customerRepository.GetAll();
            
            if(!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name));

            var total = query.Count();
            var items = query
                .OrderBy(c => c.Name)
                .Skip((page-1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerDto(c.Name, c.Phone, c.Email))
                .ToList();
            
            Response.Headers.Add("X-Total-Count", total.ToString());
            return Ok(items);
        }
}