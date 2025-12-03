namespace BlazorApp1.Services;

public class AuthService
{
    private readonly HttpClient http;

    public AuthService()
    {
        http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5088/")
        };
    }

    public async Task<string> LoginAndReturnToken(int phone, string password)
    {
        var response = await http.PostAsJsonAsync("api/Auth/login", new
        {
            Phone = phone, Password = password
        });
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Login failed: {errorContent}");
    }
        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return tokenResponse?.Token ?? "No Token Recieved";
}

    private class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}