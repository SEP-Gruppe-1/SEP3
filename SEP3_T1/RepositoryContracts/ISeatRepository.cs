using Entities;

namespace RepositoryContracts;

public interface ISeatRepository
{
  
    Task<Seat> AddAsync(Seat seat);
    Task UpdateAsync(Seat seat);
    Task DeleteAsync(int id);
    Task<Seat?> GetSingleAsync(int id);
    IQueryable<Seat> GetAll();
    
}