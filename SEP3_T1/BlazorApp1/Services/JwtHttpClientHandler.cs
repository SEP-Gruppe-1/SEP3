using Microsoft.JSInterop;
using System.Net.Http.Headers;

public class JwtHttpClientHandler
{
    private readonly IJSRuntime js;
    private bool jsReady;

    public JwtHttpClientHandler(IJSRuntime js)
    {
        this.js = js;
    }

    // KALDES EFTER FØRSTE RENDER
    public void MarkJsReady()
    {
        jsReady = true;
    }

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