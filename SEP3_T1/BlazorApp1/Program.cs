using BlazorApp1.Components;
using BlazorApp1.Services;
using gRPCRepositories;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// TILFØJ ALLE DINE SERVICES HER - FØR builder.Build()!
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5099")
});

builder.Services.AddScoped<ICustomerService, HttpCustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerInDatabaseRepository>();
// Eller hvis du har en rigtig implementering:
// builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton(new CinemaServiceClient("http://localhost:9090"));

// FØRST HER KALDER DU builder.Build()
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Tilføj denne linje hvis den mangler
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();