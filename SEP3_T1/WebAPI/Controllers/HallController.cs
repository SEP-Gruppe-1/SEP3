using ApiContract;
using gRPCRepositories;
using Microsoft.AspNetCore.Mvc;
using RepositoryContract;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HallController : ControllerBase
{
    private readonly IHallRepository hallRepository;

    public HallController(IHallRepository hallRepository)
    {
        this.hallRepository = hallRepository;
    }

    /// <summary>
    /// Get all halls
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public async Task<IActionResult> GetAllHalls()
    {
        if (hallRepository is HallInDatabaseRepository repo) await repo.Initialize();

        var halls = hallRepository.GetAll().ToList();
        return Ok(halls);
    }
    
    /// <summary>
    /// Get hall by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HallDto>> GetHallById(int id)
    {
        try
        {
            var hall = await hallRepository.getHallbyidAsync(id);
            return Ok(new HallDto(hall.Id, hall.Number, hall.LayoutId));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}