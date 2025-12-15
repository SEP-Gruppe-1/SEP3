using Microsoft.JSInterop;

public class AuthService
{
    private readonly HttpClient http;
    private readonly IJSRuntime js;

    public AuthService(IJSRuntime js)
    {
        this.js = js;
        http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5099/")
        };
    }

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

  
    public async Task LogoutAsync()
    {
        await js.InvokeVoidAsync("localStorage.removeItem", "jwt");
    }

    private class TokenResponse
    {
        public string Token { get; set; } = "";
    }
}