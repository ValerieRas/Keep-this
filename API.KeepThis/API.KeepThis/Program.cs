using API.KeepThis.Data;
using API.KeepThis.Helpers;
using API.KeepThis.Repositories;
using API.KeepThis.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

//Connection string to dataBase

string? connectionString = configuration.GetConnectionString("ConnectDB");
builder.Services.AddDbContext<IKeepThisDbContext, KeepThisDbContext>(
    options => options.UseNpgsql(connectionString)
    );

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
