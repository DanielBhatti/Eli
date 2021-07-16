using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Security
{
    public class HashMaker
    {
        public string GenerateSalt()
        {
            byte[] bytes = new byte[12];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public string GenerateHash(byte[] bytes, byte[] salt, int iterations = 10000)
        {
            Rfc2898DeriveBytes result = new Rfc2898DeriveBytes(bytes, salt, iterations);
            return Convert.ToBase64String(result.GetBytes(24));
        }

        public string GenerateHash(string password, string salt)
        {
            return GenerateHash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt));
        }
    }
}
