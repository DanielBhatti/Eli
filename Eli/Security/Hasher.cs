using System;
using System.Security.Cryptography;
using System.Text;

namespace Eli.Security;

public class Hasher
{
    public string GenerateSalt()
    {
        var bytes = new byte[12];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    public string Hash(byte[] bytes, byte[] salt, int iterations = 10000)
    {
        var hashAlgorithm = HashAlgorithmName.SHA256;
        var outputBytes = Rfc2898DeriveBytes.Pbkdf2(bytes, salt, iterations, hashAlgorithm, 24);
        return Convert.ToBase64String(outputBytes);
    }

    public string Hash(string password, string salt) => Hash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt));
}
