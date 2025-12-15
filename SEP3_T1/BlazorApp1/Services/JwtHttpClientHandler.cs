using Microsoft.JSInterop;
using System.Net.Http.Headers;

public class JwtHttpClientHandler
{
    private readonly IJSRuntime js;
    private bool jsReady;

    /// <summary>
    /// JwtHttpClientHandler Constructor
    /// </summary>
    /// <param name="js"></param>
    public JwtHttpClientHandler(IJSRuntime js)
    {
        this.js = js;
    }
    public void MarkJsReady()
    {
        jsReady = true;
    }
    
    /// <summary>
    /// Attach JWT to HttpClient
    /// </summary>
    /// <param name="client"></param>

    public async Task AttachJwtAsync(HttpClient client)
    {
        if (!jsReady)
            return; // 👈 IGNORER under prerender

        var token = await js.InvokeAsync<string>(
            "localStorage.getItem", "jwt"
        );

        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}