using ApiContract;
using Entities;
using Grpc.Core;
using gRPCRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    [HttpGet("{phone}")]
    public async Task<ActionResult<CustomerDto>> GetSingle(string phone)
    {
        try
        {
            var customer = await customerRepository.GetSingleAsync(phone);
            return Ok(new CustomerDto(customer.Name, customer.Phone, customer.Email, customer.Role));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        if (customerRepository is CustomerInDatabaseRepository repo) await repo.InitializeAsync();

        var customers = customerRepository.GetAll().ToList();
        return Ok(customers);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> AddCustomer([FromBody] CustomerCreateDto request)
    {
        Customer customer = new()
        {
            Phone = request.Phone,
            Email = request.Email,
            Password = request.Password,
            Name = request.Name,
            Role = request.Role
        };
        try
        {
            await customerRepository.VerifyCustomerDoesNotExist(request.Phone, request.Email);
            await customerRepository.SaveCustomer(customer);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {

                message = ex.Message
            });
        }
        catch (RpcException ex) when (ex.Status.Detail != null &&
                                      ex.Status.Detail.Contains("customer_email_key"))
        {
            // Simpelt fix for email-duplicate – ligesom med phone
            return Conflict(new
            {
                message = $"Email: {request.Email} already exists."
            });
        }

        await customerRepository.SaveCustomer(customer);
        SaveCustomerDto dto = new(
            request.Name,
            request.Phone,
            request.Email,
            request.Password,
            request.Role
        );
        return Created($"/api/customer/{dto.Phone}", dto);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("role")]
    public async Task<IActionResult> UpdateCustomerRole([FromBody] UpdateCustomerRoleDto dto)
    {
        var customer = await customerRepository.GetSingleAsync(dto.Phone);
        if (customer == null)
            return NotFound();

        customer.Role = dto.NewRole;
        await customerRepository.UpdateAsync(customer);

        return Ok();
    }

    [HttpDelete("{phone}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCustomer(string phone)
    {
        try
        {
            await customerRepository.DeleteAsync(phone);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
       
    }
    
    [Authorize]
    [HttpGet("bookings/{phone}")]
    public async Task<IActionResult> GetBookingsForCustomer(
        string phone,
        [FromServices] IScreeningRepository screeningRepo)
    {
        var bookings = await screeningRepo.GetBookingsByPhoneAsync(phone);
        return Ok(bookings);
    }



    

}