using Entities;
using RepositoryContracts;

namespace gRPCRepositories;

public class SeatInDatabaseRepository : ISeatRepository
{
    public Task<Seat> AddAsync(Seat seat)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Seat seat)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Seat?> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Seat> GetAll()
    {
        throw new NotImplementedException();
    }
}