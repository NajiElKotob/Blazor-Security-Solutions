using AspNetJWTAuthWebAPIDemo.Models;
using AspNetJWTAuthWebAPIDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AuthenticationService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
    });

builder.Services.AddAuthorization();


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


// ============ / Endpoint Start ============

app.MapGet("/secure", [Authorize] () => "This is a secure endpoint (Demo).");

/// <summary>
/// Generates a secure JWT key for demonstration purposes only.
/// NOT intended for production use.
/// </summary>
app.MapGet("/GenerateJwtKey", (int? length) =>
{

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


app.MapPost("/auth", (UserLoginRequest userLoginRequest,
                AuthenticationService authService,
                IConfiguration configuration) =>
{

    // Validate the username/password
    var user = authService.ValidateUserCredentials(
        userLoginRequest.Username,
        userLoginRequest.Password);

    if (user == null)
    {
        return Results.Unauthorized();
    }


    // Create a token
    var securityKey = new SymmetricSecurityKey(
        Encoding.ASCII.GetBytes(configuration["JwtSettings:Key"]));
    var signingCredentials = new SigningCredentials(
        securityKey, SecurityAlgorithms.HmacSha256);

    var claimsForToken = new List<Claim>();
    claimsForToken.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()));
    claimsForToken.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName));
    claimsForToken.Add(new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName));
    claimsForToken.Add(new Claim("city", user.City));

    var jwtSecurityToken = new JwtSecurityToken(
        configuration["Authentication:Issuer"],
        configuration["Authentication:Audience"],
        claimsForToken,
        DateTime.UtcNow,
        DateTime.UtcNow.AddHours(1),
        signingCredentials);

    var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

    return Results.Ok(tokenToReturn);
});

// ============ / Endpoint End ============



app.Run();

