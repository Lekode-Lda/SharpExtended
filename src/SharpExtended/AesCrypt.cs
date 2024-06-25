using System.Security.Cryptography;
using System.Text;

// ReSharper disable MemberCanBePrivate.Global

namespace SharpExtended;

public static class AesCrypt {

    public static string Encrypt(string plaintext, string key, string iv) =>
        Convert.ToBase64String(EncryptString(plaintext, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv)));

    public static string Decrypt(string base64, string key, string iv) =>
        DecryptString(Convert.FromBase64String(base64), Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));
    
    public static byte[] EncryptString(string plainText, byte[] key, byte[] iv) {
        if (plainText is not { Length: > 0 })
            throw new ArgumentNullException(nameof(plainText));
        if (key is not { Length: > 0 })
            throw new ArgumentNullException(nameof(key));
        if (iv is not { Length: > 0 })
            throw new ArgumentNullException(nameof(iv));

        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV  = iv;

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var          msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (var swEncrypt = new StreamWriter(csEncrypt))
            swEncrypt.Write(plainText);
        var encrypted = msEncrypt.ToArray();

        return encrypted;
    }

    public static string DecryptString(byte[] cipherText, byte[] key, byte[] iv) {
        if (cipherText is not { Length: > 0 })
            throw new ArgumentNullException(nameof(cipherText));
        if (key is not { Length: > 0 })
            throw new ArgumentNullException(nameof(key));
        if (iv is not { Length: > 0 })
            throw new ArgumentNullException(nameof(iv));

        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV  = iv;

        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(cipherText);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        var   plaintext = srDecrypt.ReadToEnd();

        return plaintext;
    }
}
