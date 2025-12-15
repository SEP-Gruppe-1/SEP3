using System.Text;
using gRPCRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RepositoryContract;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new CinemaServiceClient("http://localhost:9090"));
builder.Services.AddScoped<ICustomerRepository, CustomerInDatabaseRepository>();
builder.Services.AddScoped<IHallRepository, HallInDatabaseRepository>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeeOrAdmin", policy =>
        policy.RequireRole("Employee", "Admin"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("CustomerOnly", policy =>
        policy.RequireRole("Customer"));

    options.AddPolicy("EmployeeOnly", policy =>
        policy.RequireRole("Employee"));
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = "YourIssuer",
        ValidAudience = "YourAudience",
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("SuperSecretThatIsMinimum32CharactersLong"))
    };
});


builder.Services.AddScoped<IScreeningRepository, ScreeningInDatabaseRepository>();
builder.Services.AddScoped<IMovieRepository, MovieInDatabaseRepository>();

var app = builder.Build();

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();