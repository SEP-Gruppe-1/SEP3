using System.Net.Http.Headers;
using Microsoft.JSInterop;

public class JwtHttpClientHandler
{
    private readonly IJSRuntime js;

    public JwtHttpClientHandler(IJSRuntime js)
    {
        this.js = js;
    }

    public async Task AttachJwtAsync(HttpClient client)
    {
        var token = await js.InvokeAsync<string>("localStorage.getItem", "jwt");

        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    }
}