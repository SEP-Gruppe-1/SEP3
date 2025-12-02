using gRPCRepositories;
using RepositoryContract;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new CinemaServiceClient("http://localhost:9090"));
builder.Services.AddScoped<ICustomerRepository, CustomerInDatabaseRepository>();
builder.Services.AddScoped<IHallRepository, HallInDatabaseRepository>();
builder.Services.AddScoped<IScreeningRepository, ScreeningInRepository>();
builder.Services.AddScoped<IMovieRepository, MovieInRepository>();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();