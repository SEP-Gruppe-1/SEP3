using BlazorApp1.Components;
using BlazorApp1.Services;
using gRPCRepositories;
using Microsoft.AspNetCore.Components.Authorization;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication();


builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<TokenAuthenticationStateProvider>());

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtHttpClientHandler>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5099")
});

builder.Services.AddScoped<ICustomerService, HttpCustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerInDatabaseRepository>();
builder.Services.AddSingleton(new CinemaServiceClient("http://localhost:9090"));
builder.Services.AddScoped<IMovieRepository, MovieInRepository>();
builder.Services.AddScoped<IScreeningRepository, ScreeningInRepository>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();