using ApiContract;
using System.Text.Json;

public class HttpMovieService : IMovieService
{
    private readonly HttpClient http;

    public HttpMovieService(HttpClient http)
    {
        this.http = http;
    }

    public async Task<List<MovieDto>> GetAllMoviesAsync()
    {
        var response = await http.GetAsync("api/movie");
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(json);

        return JsonSerializer.Deserialize<List<MovieDto>>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<MovieDto?> GetMovieByIdAsync(int id)
    {
        var response = await http.GetAsync($"api/movie/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<MovieDto>();
    }
}