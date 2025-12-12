namespace BlazorApp1.Services;

/// <summary>
/// Authservice to handle login and token retrieval
/// </summary>

public class AuthService
{
    private readonly HttpClient http;

    public AuthService()
    {
        http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5099/")
        };
    }
    
    /// <summary>
    /// login and return JWT token
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>

    public async Task<string> LoginAndReturnToken(string? phone, string password)
    {
        var response = await http.PostAsJsonAsync("api/Auth/login", new
        {
            Phone = phone,
            Password = password
        });
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Login failed: {errorContent}");
        }

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return tokenResponse?.Token ?? "No Token Recieved";
    }
    
    /// <summary>
    /// Token response model
    /// </summary>

    private class TokenResponse
    {
        public string Token { get; set; }
    }
}