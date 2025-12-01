using Entities;

namespace RepositoryContracts;

public interface IMovieRepository
{
    Task<Movie> AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(int id);
    Task<Movie?> GetSingleAsync(int id);
    IQueryable<Movie> GetAll();
    
    
}