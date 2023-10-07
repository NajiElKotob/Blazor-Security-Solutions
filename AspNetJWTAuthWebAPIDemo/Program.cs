using AspNetJWTAuthWebAPIDemo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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


app.MapPost("/auth", (UserLoginRequest userLoginRequest) =>
{

    // Validate the username/password
    var user = ValidateUserCredentials(
        userLoginRequest.Username,
        userLoginRequest.Password);

    if (user == null)
    {
        return Results.Unauthorized();
    }


    string token = "YourGeneratedJWTToken";
    return Results.Ok(token);

});


UserProfile ValidateUserCredentials(string? userName, string? password)
{
    // ... existing code ...

    return new UserProfile()
    {
        UserId = 1,
        UserName = userName,
        FirstName = "Peter",
        LastName = "Smith",
        Email = "peter.smith@example.net",
        City = "Hamilton",
        LastSignInAt = DateTime.UtcNow
    };
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.Run();

