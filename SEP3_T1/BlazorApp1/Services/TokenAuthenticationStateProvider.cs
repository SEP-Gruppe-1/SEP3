using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorApp1.Services;


/// <summary>
/// Token Authentication State Provider
/// </summary>
public class TokenAuthenticationStateProvider : AuthenticationStateProvider
{
    private AuthenticationState state = new(new ClaimsPrincipal(new ClaimsIdentity()));
/// <summary>
/// Get Authentication State Asynchronously
/// </summary>
/// <returns></returns>
    public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(state);
/// <summary>
/// Sign In with JWT
/// </summary>
/// <param name="jwt"></param>
/// <returns></returns>
    public Task SignIn(string jwt)
    {

        var claims = ParseClaimsFromJwt(jwt);

        var identity = new ClaimsIdentity(
            claims,
            "jwt",
            ClaimTypes.Name,
            ClaimTypes.Role);

        var user = new ClaimsPrincipal(identity);
        state = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(state));
        

        return Task.CompletedTask;
    }

    /// <summary>
    /// Sign Out
    /// </summary>
    /// <returns></returns>
    
    public Task SignOut()
    {
        state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        NotifyAuthenticationStateChanged(Task.FromResult(state));
        return Task.CompletedTask;
    }
    
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        return token.Claims;
    }
    
    /// <summary>
    /// Load JWT from Storage
    /// </summary>
    /// <param name="js"></param>
    
    public async Task LoadJwtFromStorage(IJSRuntime js)
    {
        var saved = await js.InvokeAsync<string>("localStorage.getItem", "jwt");
        if (!string.IsNullOrEmpty(saved))
        {
            await SignIn(saved);
        }
    }


  
}