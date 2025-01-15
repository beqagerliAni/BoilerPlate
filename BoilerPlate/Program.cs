using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using todolist.Helper.Auth.Service;
using todolist.Helper.Configuration;
using todolist.src.Modules.User.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                      .RequireAuthenticatedUser()
                      .Build();
    config.Filters.Add(new AuthorizeFilter(policy));

});
builder.Services.AddScoped<appDbcontext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));
string? connectionstring = builder.Configuration["ConnectionStrings:AppDbConnectionString"];
builder.Services.AddDbContext<appDbcontext>(options =>
    options.UseMySql(connectionstring, serverVersion));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

Configuration._configuration = builder.Configuration;

builder.Services.AddOpenApi();

// Configure the HTTP request pipeline.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        string? key = builder.Configuration["SecurityKey"];
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key), "SecurityKey is missing in appsettings.json");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Audience"]
        };
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
        // https://localhost:5078/scalar/v1
    app.MapScalarApiReference();

}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
