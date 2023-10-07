using AspNetJWTAuthWebAPIDemo.Models;

namespace AspNetJWTAuthWebAPIDemo.Services
{
    public class AuthenticationService
    {
     public UserProfile ValidateUserCredentials(string? userName, string? password)
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
    }
}
