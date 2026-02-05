using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DocumentManagement.Helper;
public static class ClientSecretGenerator
{
    public static string GenerateClientSecret()
    {
        var secretBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(secretBytes);
        }
        var secret = WebEncoders.Base64UrlEncode(secretBytes);
        return HashSecret(secret);
    }

    private static string HashSecret(string secret)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(secret));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
