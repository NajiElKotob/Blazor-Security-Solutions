using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BlazorSessionAuthDemo.Utilities;

public static class EncryptionHelper
{
    public static string HashPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }

    public static bool VerifyPasswordHash(string password, string storedHash)
    {
        string[] parts = storedHash.Split('.');
        if (parts.Length != 2)
        {
            throw new FormatException("Unexpected hash format");
        }

        byte[] salt = Convert.FromBase64String(parts[0]);
        string hash = parts[1];

        string expectedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hash == expectedHash;
    }
}
