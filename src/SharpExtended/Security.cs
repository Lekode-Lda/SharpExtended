using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SharpExtended;

public static class Security {
        /// <summary>
    /// Generates a salted hash using SHA256
    /// </summary>
    /// <param name="plainText">Text to be hashed</param>
    /// <param name="salt">Salt</param>
    /// <returns>Salted hash of the string</returns>
    public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt) {

        var plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

        for (var i = 0; i < plainText.Length; i++)
            plainTextWithSaltBytes[i] = plainText[i];

        for (var i = 0; i < salt.Length; i++)
            plainTextWithSaltBytes[plainText.Length + i] = salt[i];

        return SHA256.HashData(plainTextWithSaltBytes);
    }

    /// <summary>
    /// Compares two byte arrays
    /// </summary>
    /// <param name="array1">Byte Array A</param>
    /// <param name="array2">Byte Array B</param>
    /// <returns></returns>
    public static bool CompareByteArrays(byte[] array1, byte[] array2) {
        if (array1.Length != array2.Length)
            return false;

        return !array1.Where((t, i) => t != array2[i]).Any();
    }

    /// <summary>
    /// Generates a new salt with a specified length in bytes
    /// </summary>
    /// <param name="maximumSaltLength">Length of the salt. Defaults to 64 bytes</param>
    /// <returns>The salt</returns>
    public static byte[] GetSalt(int maximumSaltLength = 64) {
        var       salt   = new byte[maximumSaltLength];
        using var rnd    = RandomNumberGenerator.Create();
        rnd.GetNonZeroBytes(salt);

        return salt;
    }

    /// <summary>
    /// Calculates the SHA1 of a string
    /// </summary>
    /// <param name="input">String to generate the hash for</param>
    /// <returns>String containing the hash in lowercased hex values</returns>
    public static string Sha1(string input) {
        var hash = SHA1.HashData(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder(hash.Length * 2);

        foreach (var b in hash)
            sb.Append(b.ToString("X2"));            

        return sb.ToString().ToLower();
    }

    /// <summary>
    /// Calculates the SHA256 of a string
    /// </summary>
    /// <param name="text">String to generate the hash for</param>
    /// <returns>String containing the hash in lowercased hex values</returns>
    public static string Sha256(string text) {
        var hashString = string.Empty;
        var bytes = Encoding.UTF8.GetBytes(text);
        var hash = SHA256.HashData(bytes);

        return hash.Aggregate(hashString, (current, x) => current + $"{x:x2}");
    }

    /// <summary>
    /// Calculates the HMAC-SHA256 of some data
    /// </summary>
    /// <param name="data">Data to be processed</param>
    /// <param name="key">Key to generate the signature</param>
    /// <returns>The HMAC-SHA256 as byte array</returns>
    public static IEnumerable<byte> HmacSha256(byte[] data, string key) {
        var bytes = Encoding.UTF8.GetBytes(key);
        using HMACSHA256 hasher = new(bytes);
        using MemoryStream stream = new(data);
        return hasher.ComputeHash(stream);
    }

    /// <summary>
    /// Calculates the HMAC-MD5 of some data (ouch...)
    /// </summary>
    /// <param name="data">Data to be processed</param>
    /// <param name="key">Key to generate the signature</param>
    /// <returns>The HMAC-MD5 as byte array</returns>
    public static IEnumerable<byte> HmacMd5(byte[] data, string key) {
        var bytes = Encoding.UTF8.GetBytes(key);
        using HMACMD5 hasher = new(bytes);
        using MemoryStream stream = new(data);
        return hasher.ComputeHash(stream);
    }

    /// <summary>
    /// Generates a MD5 hash
    /// </summary>
    /// <param name="data">Data to hash</param>
    /// <returns>MD5 hash</returns>
    public static byte[] Md5(byte[] data) => MD5.HashData(data);
}
