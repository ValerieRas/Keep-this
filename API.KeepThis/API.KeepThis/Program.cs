using API.KeepThis.Data;
using API.KeepThis.Helpers;
using API.KeepThis.Repositories;
using API.KeepThis.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.docker.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

//Connection string to dataBase

string? connectionString = configuration.GetConnectionString("ConnectDB");
builder.Services.AddDbContext<KeepThisDbContext>(
    options => options.UseNpgsql(connectionString).EnableDetailedErrors()
    );


// Load JwtSettings from configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Register the interface with the configured implementation
builder.Services.AddScoped<IJwtSettings>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;
    return options;
});

// Register AuthentificationService
builder.Services.AddScoped<IAuthentificationService>(serviceProvider =>
{

    var jwtSettings = serviceProvider.GetRequiredService<IJwtSettings>();
    var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();
    var usersRepository = serviceProvider.GetRequiredService<IUsersRepository>();
    var authTokenRepository = serviceProvider.GetRequiredService<IAuthTokenRepository>();

    // Create AuthentificationService
    return new AuthentificationService(jwtSettings, passwordHasher, usersRepository, authTokenRepository);
});


string secretKey = builder.Configuration["Jwt:SecretKey"];
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Set to true in production
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthentificationService, AuthentificationService>();
builder.Services.AddScoped<IAuthTokenRepository, AuthTokenRepository>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
 );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
