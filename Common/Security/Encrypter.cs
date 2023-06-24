using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Common.Security;

//https://stackoverflow.com/a/10177020/6732239
public class Encrypter
{
    // This constant is used to determine the keysize of the encryption algorithm in bits.
    // We divide this by 8 within the code below to get the equivalent number of bytes.
    private const int Keysize = 128;

    // This constant determines the number of iterations for the password bytes generation function.
    private const int DerivationIterations = 1000;

    public string Encrypt(string plainText, string passPhrase)
    {
        // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
        // so that the same Salt and IV values can be used when decrypting.  
        var saltStringBytes = Generate128BitsOfRandomEntropy();
        var ivStringBytes = Generate128BitsOfRandomEntropy();
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        using Rfc2898DeriveBytes password = new(passPhrase, saltStringBytes, DerivationIterations, HashAlgorithmName.MD5);
        var keyBytes = password.GetBytes(Keysize / 8);
        using var symmetricKey = Aes.Create();
        symmetricKey.BlockSize = 128;
        symmetricKey.Mode = CipherMode.CBC;
        symmetricKey.Padding = PaddingMode.PKCS7;
        using var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes);
        using MemoryStream memoryStream = new();
        using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.FlushFinalBlock();
        // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
        var cipherTextBytes = saltStringBytes;
        cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
        cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
        memoryStream.Close();
        cryptoStream.Close();
        return Convert.ToBase64String(cipherTextBytes);
    }

    public string Decrypt(string cipherText, string passPhrase, bool hasPadding = false)
    {
        // Get the complete stream of bytes that represent:
        // [16 bytes of Salt] + [16 bytes of IV] + [n - 32 bytes of CipherText]
        var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
        // Get the saltbytes by extracting the first 16 bytes from the supplied cipherText bytes.
        var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
        // Get the IV bytes by extracting the next 16 bytes from the supplied cipherText bytes.
        var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
        // Get the actual cipher text bytes by removing the first 32 bytes from the cipherText string.
        var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8 * 2).Take(cipherTextBytesWithSaltAndIv.Length - Keysize / 8 * 2).ToArray();

        using Rfc2898DeriveBytes password = new(passPhrase, saltStringBytes, DerivationIterations, HashAlgorithmName.MD5);
        var keyBytes = password.GetBytes(Keysize / 8);
        using var symmetricKey = Aes.Create();
        symmetricKey.BlockSize = 128;
        symmetricKey.Mode = CipherMode.CBC;
        symmetricKey.Padding = PaddingMode.PKCS7;
        using var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes);
        using MemoryStream memoryStream = new(cipherTextBytes);
        using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
        var plainTextBytes = new byte[cipherTextBytes.Length];
        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        memoryStream.Close();
        cryptoStream.Close();
        var result = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        return hasPadding ? result : result.Trim('\"');
    }

    private byte[] Generate128BitsOfRandomEntropy()
    {
        var randomBytes = new byte[16]; // 16 Bytes will give us 128 bits.
        using var rngCsp = RandomNumberGenerator.Create();
        rngCsp.GetBytes(randomBytes);
        return randomBytes;
    }

    public string JsonEncrypt(object o, string password)
    {
        var json = JsonSerializer.Serialize(o);
        var cipher = Encrypt(json, password);
        return cipher;
    }

    public object JsonDecrypt(string cipher, string password, Type type)
    {
        var plainText = Decrypt(cipher, password);
        return JsonSerializer.Deserialize(plainText, type);
    }
}
