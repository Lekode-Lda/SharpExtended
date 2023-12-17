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
    /// Generates a RSA Key Pair
    /// </summary>
    /// <param name="size">Size of the keys. Defaults to 2048</param>
    /// <returns>RSA Keys</returns>
    public static RsaPublicPrivateKeyPair GenerateRsaKeyPair(int size = 2048) {
        var secureRandom = new SecureRandom();
        var keyGenerationParameters = new KeyGenerationParameters(secureRandom, size);

        var keyPairGenerator = new RsaKeyPairGenerator();
        keyPairGenerator.Init(keyGenerationParameters);
        var keys = keyPairGenerator.GenerateKeyPair();

        return new RsaPublicPrivateKeyPair {
            Public = ExportKeyPair(keys, KeyType.Public),
            Private = ExportKeyPair(keys, KeyType.Private)
        };
    }

    /// <summary>
    /// Encrypts text with a RSA Public Key
    /// </summary>
    /// <param name="clearText">Text to encrypt</param>
    /// <param name="publicKey">Public key</param>
    /// <returns>Encrypted string</returns>
    public static string RsaEncryptWithPublic(string clearText, string publicKey) {
        var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

        var encryptEngine = new Pkcs1Encoding(new RsaEngine());

        using (var txtReader = new StringReader(publicKey)) {
            var keyParameter = (AsymmetricKeyParameter)new PemReader(txtReader).ReadObject();

            encryptEngine.Init(true, keyParameter);
        }

        var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
        return encrypted;

    }

    /// <summary>
    /// Encrypts text with a RSA Private Key
    /// </summary>
    /// <param name="clearText">String to encrypt</param>
    /// <param name="privateKey">Private Key</param>
    /// <returns>Encrypted string</returns>
    public static string RsaEncryptWithPrivate(string clearText, string privateKey) {
        var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

        var encryptEngine = new Pkcs1Encoding(new RsaEngine());

        using (var txtReader = new StringReader(privateKey)) {
            var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtReader).ReadObject();

            encryptEngine.Init(true, keyPair.Private);
        }

        var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
        return encrypted;
    }

    /// <summary>
    /// Decrypts a string with a RSA Private Key
    /// </summary>
    /// <param name="base64Input">Base64 string containing the encrypted text</param>
    /// <param name="privateKey">The Private Key</param>
    /// <returns>Decrypted text</returns>
    public static string RsaDecryptWithPrivate(string base64Input, string privateKey) {
        var bytesToDecrypt = Convert.FromBase64String(base64Input);

        var decryptEngine = new Pkcs1Encoding(new RsaEngine());

        using (var txtReader = new StringReader(privateKey)) {
            var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtReader).ReadObject();

            decryptEngine.Init(false, keyPair.Private);
        }

        var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
        return decrypted;
    }

    /// <summary>
    /// Decrypts a string with a RSA Public Key
    /// </summary>
    /// <param name="base64Input">Base64 string containing the encrypted text</param>
    /// <param name="publicKey">The Public Key</param>
    /// <returns>Decrypted text</returns>
    public static string RsaDecryptWithPublic(string base64Input, string publicKey) {
        var bytesToDecrypt = Convert.FromBase64String(base64Input);

        var decryptEngine = new Pkcs1Encoding(new RsaEngine());

        using (var txtReader = new StringReader(publicKey)) {
            var keyParameter = (AsymmetricKeyParameter)new PemReader(txtReader).ReadObject();

            decryptEngine.Init(false, keyParameter);
        }

        var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
        return decrypted;
    }

    /// <summary>
    /// Exports the RSA Key Pair
    /// </summary>
    /// <param name="keyPair">RSA Key Pair</param>
    /// <param name="extractKey">Key to extract. See <see cref="KeyType"/> for the key types</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static string ExportKeyPair(AsymmetricCipherKeyPair keyPair, KeyType extractKey) {
        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        var pemWriter = new PemWriter(streamWriter);

        switch (extractKey) {
            case KeyType.Public:
                pemWriter.WriteObject(keyPair.Public);
                break;
            case KeyType.Private:
                pemWriter.WriteObject(keyPair.Private);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(extractKey), extractKey, null);
        }

        streamWriter.Flush();

        return Encoding.ASCII.GetString(memoryStream.GetBuffer());
    }

    /// <summary>
    /// Generates a MD5 hash
    /// </summary>
    /// <param name="data">Data to hash</param>
    /// <returns>MD5 hash</returns>
    public static byte[] Md5(byte[] data) => MD5.HashData(data);

    public class RsaPublicPrivateKeyPair {
        public string Public  { get; set; }
        public string Private { get; set; }
    }

    public enum KeyType {
        Public,
        Private
    }
}