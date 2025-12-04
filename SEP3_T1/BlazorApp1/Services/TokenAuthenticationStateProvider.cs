using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorApp1.Services;

public class TokenAuthenticationStateProvider : AuthenticationStateProvider
{
    private AuthenticationState state = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(state);

    public Task SignIn(string jwt)
    {
        var claims = ParseClaimsFromJwt(jwt);
        var identity = new ClaimsIdentity(claims, "jwt", nameType: "name", roleType: "role");
        var user = new ClaimsPrincipal(identity);
        Console.WriteLine("SignIn called with: " + jwt);
        state = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(state));
        return Task.CompletedTask;
        
    }
    
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
    
    public async Task LoadJwtFromStorage(IJSRuntime js)
    {
        var saved = await js.InvokeAsync<string>("localStorage.getItem", "jwt");
        if (!string.IsNullOrEmpty(saved))
        {
            await SignIn(saved);
        }
    }


  
}