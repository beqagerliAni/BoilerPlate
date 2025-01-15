using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using To_do_List.src.Modules.User.Command;
using todolist.Helper.Auth.Service;
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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

string? connectionstring = builder.Configuration.GetConnectionString("AppDbConnectionString");
Console.WriteLine(connectionstring);
var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


//add mediator :) 

//builder.Services.AddMediatR();
//builder.Services.AddMediatR();

builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)

.AddJwtBearer(options =>
{

    string? key = builder.Configuration.GetConnectionString("SecurityKey");
    if (key == null) { throw new ArgumentNullException(nameof(key)); }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetConnectionString("Issuer"),
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetConnectionString("Audience")
    };
});
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
