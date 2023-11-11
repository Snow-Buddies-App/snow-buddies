using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using SnowBuddies.Application.AutoMapperConfiguration;
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
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddDbContext<SnowBuddiesDbContext>(options => options.UseNpgsql(fullConnectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program), typeof(ApplicationProfile));

var jwtSettings = builder.Configuration.GetSection("JWTSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.GetValue<string>("validIssuer"),
        ValidAudience = jwtSettings.GetValue<string>("validAudience"),
        IssuerSigningKey = new SymmetricSecurityKey((new System.Text.UTF8Encoding()).GetBytes(jwtSettings.GetValue<string>("securityKey").ToCharArray()))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
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
