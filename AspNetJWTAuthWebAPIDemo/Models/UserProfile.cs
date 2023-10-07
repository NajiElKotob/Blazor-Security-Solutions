namespace AspNetJWTAuthWebAPIDemo.Models;

public class UserProfile
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
    public DateTime LastSignInAt { get; set; }

    public UserProfile() { }

    public UserProfile(
        int userId,
        string userName,
        string firstName,
        string lastName,
        string email,
        string city,
        DateTime lastSignInAt)
    {
        UserId = userId;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        City = city;
        LastSignInAt = lastSignInAt;
    }
}
