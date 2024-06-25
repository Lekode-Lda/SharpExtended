using System.Security.Cryptography;
using System.Text;

namespace SharpExtended.Rsa;

public static class RsaCrypt {

    /// <summary>
    /// Generates a RSA Key Pair
    /// </summary>
    /// <param name="size">Size of the keys. Defaults to 2048</param>
    /// <returns><see cref="KeyPair"/></returns>
    public static KeyPair GenerateKey(KeySize size = KeySize.L2048) {
        var rsa = RSA.Create((int)size);
        return new KeyPair(rsa.ExportRSAPublicKeyPem(), rsa.ExportRSAPrivateKeyPem());
    }

    /// <summary>
    /// Encrypts the string with the public key
    /// </summary>
    /// <param name="data">String to encrypt</param>
    /// <param name="publicKey">Public key in PEM format</param>
    /// <param name="padding">Encryption Padding, defaults to PKCS1</param>
    /// <returns>The BASE64 representation of the encryption data</returns>
    public static string Encrypt(string data, string publicKey, RSAEncryptionPadding? padding = null) {
        var rsa = RSA.Create();
        rsa.ImportFromPem(publicKey);
        return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(data), padding ?? RSAEncryptionPadding.Pkcs1));
    }

    /// <summary>
    /// Decrypts the string with the private key
    /// </summary>
    /// <param name="base64Data">Base64 string to decrypt</param>
    /// <param name="privateKey">Private key in PEM format</param>
    /// <param name="padding">Encryption Padding, defaults to PKCS1</param>
    /// <returns>Plain text</returns>
    public static string Decrypt(string base64Data, string privateKey, RSAEncryptionPadding? padding = null) {
        var rsa = RSA.Create();
        rsa.ImportFromPem(privateKey);
        return Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(base64Data), padding ?? RSAEncryptionPadding.Pkcs1));
    }

    /// <summary>
    /// Enum with the possible key sizes
    /// </summary>
    public enum KeySize {
        L512 = 512,
        L1024 = 1024,
        L2048 = 2048,
        L3072 = 3072,
        L4096 = 4096
    }

}
