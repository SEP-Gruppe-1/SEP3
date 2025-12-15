using ApiContract;
using System.Net.Http.Json;

namespace BlazorApp1.Services;

public class HttpMovieService : IMovieService
{
    private readonly HttpClient http;
    private readonly JwtHttpClientHandler jwtHandler;

    public HttpMovieService(HttpClient http, JwtHttpClientHandler jwtHandler)
    {
        this.http = http;
        this.jwtHandler = jwtHandler;
    }

    public async Task<List<MovieDto>> GetAllMoviesAsync()
    {
        await jwtHandler.AttachJwtAsync(http);

        var response = await http.GetAsync("api/movie");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Kunne ikke hente film");

        return await response.Content.ReadFromJsonAsync<List<MovieDto>>() ?? new();
    }
    
    public async Task<MovieDto?> GetMovieByIdAsync(int id)
    {
        var response = await http.GetAsync($"api/movie/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<MovieDto>();
    }

    public async Task<List<MovieDto>> SearchMoviesAsync(string query)
    {
        await jwtHandler.AttachJwtAsync(http);

        var response = await http.GetAsync(
            $"api/movie?query={Uri.EscapeDataString(query)}"
        );

        if (!response.IsSuccessStatusCode)
            throw new Exception("Kunne ikke hente film");

        return await response.Content.ReadFromJsonAsync<List<MovieDto>>() ?? new();
    }
    
    public async Task CreateMovieAsync(MovieCreateDto dto)
    {
        await jwtHandler.AttachJwtAsync(http);

        var response = await http.PostAsJsonAsync("api/movie", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}