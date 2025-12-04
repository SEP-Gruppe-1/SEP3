using Entities;
using RepositoryContracts;

namespace gRPCRepositories;

public class LayoutInDatabaseRepository : ILayoutRepository
{
    private readonly CinemaServiceClient _client;
    private List<Layout> layouts;
    
    public LayoutInDatabaseRepository(CinemaServiceClient _client)
    {
        this._client = _client;
        layouts = new List<Layout>();
    }


    public Task<Layout> AddAsync(Layout layout)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Layout layout)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Layout?> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Layout> GetAll()
    {
        throw new NotImplementedException();
    }
}