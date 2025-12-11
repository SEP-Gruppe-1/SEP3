using ApiContract;
using Entities;


namespace RepositoryContracts;

public interface IScreeningRepository
{
    Task<Screening> AddAsync(ScreeningCreateDto dto);
    Task updateAsync(Screening screening);
    Task deleteAsync(int id);
    Task<Screening?> getSingleAsync(int id);
    Task<List<Screening>> getAll();
    
    Task BookSeatsAsync(int screeningId, List<int> seatIds, string phoneNumber);
    Task UpdateBookingAsync(int screeningId, string phoneNumber, List<int> seatsToAdd, List<int> seatsToRemove);
}