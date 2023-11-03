using Microsoft.EntityFrameworkCore;
using Npgsql;
using SnowBuddies.Application.Implementation.Services;
using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Infrastructure.Data;
using SnowBuddies.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);


var connectionStringFromConfig = builder.Configuration.GetConnectionString("SBConnectionString");
string? dbPassword = builder.Configuration["Password"];

var npgsqlConStrBuilder = new NpgsqlConnectionStringBuilder(connectionStringFromConfig)
{
    Password = dbPassword
};
var fullConnectionString = npgsqlConStrBuilder.ToString();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

builder.Services.AddDbContext<SnowBuddiesDbContext>(options => options.UseNpgsql(fullConnectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));


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
