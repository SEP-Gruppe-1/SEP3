using Entities;

namespace RepositoryContracts;

public interface IScreeningRepository
{
    Task<Screening> AddAsync(Screening screening);
    Task updateAsync(Screening screening);
    Task deleteAsync(int id);
    Task<Screening?> getSingleAsync(int id);
    Task<List<Screening>> getAll();
    
    Task BookSeatsAsync(int screeningId, List<int> seatIds, string phoneNumber);
}