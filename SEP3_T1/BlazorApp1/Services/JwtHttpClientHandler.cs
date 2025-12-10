using System.Net.Http.Headers;
using Microsoft.JSInterop;


/// <summary>
/// JWT HttpClient Handler
/// </summary>
public class JwtHttpClientHandler
{
    private readonly IJSRuntime js;

    public JwtHttpClientHandler(IJSRuntime js)
    {
        this.js = js;
    }
    
    /// <summary>
    /// Attach JWT token to HttpClient
    /// </summary>
    /// <param name="client"></param>

    public async Task AttachJwtAsync(HttpClient client)
    {
        var token = await js.InvokeAsync<string>("localStorage.getItem", "jwt");

        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}