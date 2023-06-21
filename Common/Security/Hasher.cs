using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Security;

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
        Rfc2898DeriveBytes result = new(bytes, salt, iterations, hashAlgorithm);
        return Convert.ToBase64String(result.GetBytes(24));
    }

    public string Hash(string password, string salt) => Hash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt));
}
