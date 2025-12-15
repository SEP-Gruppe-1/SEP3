using Microsoft.JSInterop;

public class AuthService
{
    private readonly HttpClient http;
    private readonly IJSRuntime js;
/// <summary>
/// Auth Service Constructor
/// </summary>
/// <param name="js"></param>
    public AuthService(IJSRuntime js)
    {
        this.js = js;
        http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5099/")
        };
    }

/// <summary>
/// Login And Return Token
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
        
        await js.InvokeVoidAsync("localStorage.setItem", "jwt", tokenResponse!.Token);

        return tokenResponse.Token;
    }
/// <summary>
/// Logout
/// </summary>
  
    public async Task LogoutAsync()
    {
        await js.InvokeVoidAsync("localStorage.removeItem", "jwt");
    }

/// <summary>
/// Token Response Class
/// </summary>

    private class TokenResponse
    {
        public string Token { get; set; } = "";
    }
}