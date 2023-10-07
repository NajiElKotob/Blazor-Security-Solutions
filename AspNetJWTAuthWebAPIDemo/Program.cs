using AspNetJWTAuthWebAPIDemo.Models;
using AspNetJWTAuthWebAPIDemo.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AuthenticationService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ================================================================

app.MapGet("/secure", [Authorize] () => "This is a secure endpoint (Demo).");

/// <summary>
/// Generates a secure JWT key for demonstration purposes only.
/// NOT intended for production use.
/// </summary>
app.MapGet("/GenerateJwtKey", (int? length) => {

    if (!length.HasValue)
    {
        length = 32;
    }

    using (var randomNumberGenerator = new RNGCryptoServiceProvider())
    {
        var keyBytes = new byte[length.Value];
        randomNumberGenerator.GetBytes(keyBytes);
        return Convert.ToBase64String(keyBytes);
    }
});


app.MapPost("/auth", (UserLoginRequest userLoginRequest, AuthenticationService authService) =>
{

    // Validate the username/password
    var user = authService.ValidateUserCredentials(
        userLoginRequest.Username,
        userLoginRequest.Password);

    if (user == null)
    {
        return Results.Unauthorized();
    }


    string token = "YourGeneratedJWTToken";
    return Results.Ok(token);

});


app.UseHttpsRedirection();

app.Run();

