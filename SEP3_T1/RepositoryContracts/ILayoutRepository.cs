using Entities;

namespace RepositoryContracts;

public interface ILayoutRepository
{
    Task<Layout> AddAsync(Layout layout);
    Task UpdateAsync(Layout layout);
    Task DeleteAsync(int id);
    Task<Layout?> GetSingleAsync(int id);
    IQueryable<Layout> GetAll();
}