using System.Security.Cryptography;
using System.Text;
// ReSharper disable MemberCanBePrivate.Global

namespace SharpExtended.Aes;

public class AesCrypt {
    
    #region Private members
    private readonly ICryptoTransform? _encryptor;
    private readonly ICryptoTransform? _decryptor;
    private readonly AesCryptOptions?  _options;
    #endregion

    #region Constructors

    /// <summary>
    /// Use this constructor to perform encryption/decryption with the following options:
    /// - 128/192/256-bit key (depending on passPhrase length in bits)
    /// - SHA-1 password hashing algorithm with 4-to-8 byte long password hash salt and 1 password iteration
    /// - hashing without salt
    /// - cipher block chaining (CBC) mode
    /// </summary>
    /// <param name="passPhrase">
    /// Passphrase (in string format) from which a pseudo-random password will be derived. The derived password will be used to generate the encryption key.
    /// </param>
    /// <param name="initVector">
    /// Initialization vector (IV). This value is required to encrypt the first block of plaintext data. IV must be exactly 16 ASCII characters long.
    /// </param>
    public AesCrypt(string  passPhrase,
                    string? initVector = null) :
        this(passPhrase, initVector, new AesCryptOptions()) {
    }

    /// <summary>
    /// Use this constructor to perform encryption/decryption with custom options.
    /// See AESCryptOptions documentation for details.
    /// </summary>
    /// <param name="passPhrase">
    /// Passphrase (in string format) from which a pseudo-random password will be derived. The derived password will be used to generate the encryption key.
    /// </param>
    /// <param name="initVector">
    /// Initialization vector (IV). This value is required to encrypt the first block of plaintext data. IV must be exactly 16 ASCII characters long.
    /// </param>
    /// <param name="options">
    /// A set of customized (or default) options to use for the encryption/decryption: see AESCryptOptions documentation for details.
    /// </param>
    public AesCrypt(string passPhrase, string? initVector, AesCryptOptions? options) {
        _options = options;
        
        if (_options is { FixedKeySize: not null }
            && _options.FixedKeySize != 128
            && _options.FixedKeySize != 192
            && _options.FixedKeySize != 256)
            throw new NotSupportedException("ERROR: options.FixedKeySize must be NULL (for auto-detect) or have a value of 128, 192 or 256");

        var initVectorBytes =
            initVector == null ? [] : Encoding.UTF8.GetBytes(initVector);

        if (_options == null) return;
        var keySize = _options.FixedKeySize ?? GetAesKeySize(passPhrase);
        
        var keyBytes = Array.Empty<byte>();
        if (_options.PasswordHash == AesPasswordHash.None)
            keyBytes = Encoding.UTF8.GetBytes(passPhrase);
        else {
            if (options != null) {
                var saltValueBytes =
                    Encoding.UTF8.GetBytes(options.PasswordHashSalt);

                var password = new PasswordDeriveBytes(
                    passPhrase,
                    saltValueBytes,
                    _options.PasswordHash.ToString().ToUpper().Replace("-", ""),
                    _options.PasswordHashIterations);

                keyBytes = password.GetBytes(keySize / 8);
            }
        }

        // Initialize AES key object.
        var symmetricKey = System.Security.Cryptography.Aes.Create();

        // Sets the padding mode
        symmetricKey.Padding = _options.PaddingMode;

        // Use the unsafe ECB cypher mode (not recommended) if no IV has been provided, otherwise use the more secure CBC mode.
        symmetricKey.Mode = (initVectorBytes.Length == 0)
            ? CipherMode.ECB
            : CipherMode.CBC;

        // Create the encryptor and decryptor objects, which we will use for cryptographic operations.
        _encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
        _decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
    }
    #endregion

    #region Encryption routines
    /// <summary>
    /// Encrypts a string value generating a base64-encoded string.
    /// </summary>
    /// <param name="plainText">
    /// Plain text string to be encrypted.
    /// </param>
    /// <returns>
    /// Cipher text formatted as a base64-encoded string.
    /// </returns>
    public string Encrypt(string plainText) {
        return Encrypt(Encoding.UTF8.GetBytes(plainText));
    }

    /// <summary>
    /// Encrypts a byte array generating a base64-encoded string.
    /// </summary>
    /// <param name="plainTextBytes">
    /// Plain text bytes to be encrypted.
    /// </param>
    /// <returns>
    /// Cipher text formatted as a base64-encoded string.
    /// </returns>
    public string Encrypt(byte[] plainTextBytes) {
        return Convert.ToBase64String(EncryptToBytes(plainTextBytes) ?? throw new Exception(message: "Unable to encrypt data"));
    }

    /// <summary>
    /// Encrypts a string value generating a byte array of cipher text.
    /// </summary>
    /// <param name="plainText">
    /// Plain text string to be encrypted.
    /// </param>
    /// <returns>
    /// Cipher text formatted as a byte array.
    /// </returns>
    public byte[]? EncryptToBytes(string plainText) {
        return EncryptToBytes(Encoding.UTF8.GetBytes(plainText));
    }

    /// <summary>
    /// Encrypts a byte array generating a byte array of cipher text.
    /// </summary>
    /// <param name="plainTextBytes">
    /// Plain text bytes to be encrypted.
    /// </param>
    /// <returns>
    /// Cipher text formatted as a byte array. If null there was a problem encrypting the data
    /// </returns>
    public byte[]? EncryptToBytes(byte[] plainTextBytes) {
        // Add salt at the beginning of the plain text bytes (if needed).
        var plainTextBytesWithSalt = UseSalt() ? AddSalt(plainTextBytes) : plainTextBytes;

        // Let's make cryptographic operations thread-safe.
        lock (this) {
            // Encryption will be performed using memory stream.
            byte[]?             cipherTextBytes = null;
            using var memoryStream    = new MemoryStream();
            // To perform encryption, we must use the Write mode.
            if (_encryptor != null) {
                using var cryptoStream = new CryptoStream(
                    memoryStream,
                    _encryptor,
                    CryptoStreamMode.Write);
                
                cryptoStream.Write(plainTextBytesWithSalt, 0, plainTextBytesWithSalt.Length);
                cryptoStream.FlushFinalBlock();
                cipherTextBytes = memoryStream.ToArray();
                cryptoStream.Close();
            }

            memoryStream.Close();
            
            return cipherTextBytes;
        }
    }
    #endregion

    #region Decryption routines
    
    /// <summary>
    /// Decrypts a base64-encoded cipher text value generating a string result.
    /// </summary>
    /// <param name="cipherText">
    /// Base64-encoded cipher text string to be decrypted.
    /// </param>
    /// <returns>
    /// Decrypted string value.
    /// </returns>
    public string Decrypt(string cipherText) {
        return Decrypt(Convert.FromBase64String(cipherText));
    }

    /// <summary>
    /// Decrypts a byte array containing cipher text value and generates a
    /// string result.
    /// </summary>
    /// <param name="cipherTextBytes">
    /// Byte array containing encrypted data.
    /// </param>
    /// <returns>
    /// Decrypted string value.
    /// </returns>
    public string Decrypt(byte[] cipherTextBytes) {
        return Encoding.UTF8.GetString(DecryptToBytes(cipherTextBytes) ?? throw new Exception(message: "Unable to decrypt data"));
    }

    /// <summary>
    /// Decrypts a base64-encoded cipher text value and generates a byte array
    /// of plain text data.
    /// </summary>
    /// <param name="cipherText">
    /// Base64-encoded cipher text string to be decrypted.
    /// </param>
    /// <returns>
    /// Byte array containing decrypted value.
    /// </returns>
    public byte[] DecryptToBytes(string cipherText) {
        return DecryptToBytes(Convert.FromBase64String(cipherText));
    }

    /// <summary>
    /// Decrypts a base64-encoded cipher text value and generates a byte array
    /// of plain text data.
    /// </summary>
    /// <param name="cipherTextBytes">
    /// Byte array containing encrypted data.
    /// </param>
    /// <returns>
    /// Byte array containing decrypted value.
    /// </returns>
    public byte[] DecryptToBytes(byte[] cipherTextBytes) {
        var decryptedByteCount = 0;
        var saltLen            = 0;
        var decryptedBytes = new byte[cipherTextBytes.Length];

        lock (this) {
            using var memoryStream = new MemoryStream(cipherTextBytes);
            if (_decryptor != null) {
                using var cryptoStream = new CryptoStream(memoryStream, _decryptor, CryptoStreamMode.Read);
                decryptedByteCount = cryptoStream.Read(
                    decryptedBytes,
                    0,
                    decryptedBytes.Length
                );
                cryptoStream.Close();
            }

            memoryStream.Close();
        }

        if (UseSalt()) {
            saltLen = (decryptedBytes[0] & 0x03) |
                      (decryptedBytes[1] & 0x0c) |
                      (decryptedBytes[2] & 0x30) |
                      (decryptedBytes[3] & 0xc0);
        }

        var plainTextBytes = new byte[decryptedByteCount - saltLen];

        Array.Copy(decryptedBytes, saltLen, plainTextBytes,
                   0, decryptedByteCount - saltLen);

        return plainTextBytes;
    }
    #endregion

    #region Helper functions
    /// <summary>
    /// Gets the KeySize by the password length in bytes.
    /// </summary>
    /// <param name="passPhrase">Passphrase to use during the decryption</param>
    /// <returns>Key Size</returns>
    private static int GetAesKeySize(string passPhrase) {
        return passPhrase.Length switch {
            16 => 128,
            24 => 192,
            32 => 256,
            _  => throw new NotSupportedException("ERROR: AES Password must be of 16, 24 or 32 bits length!")
        };
    }

    /// <summary>
    /// Checks if salt must be used or not for the encryption/decryption.
    /// </summary>
    /// <returns></returns>
    private bool UseSalt() =>
        _options is { MaxSaltLength: > 0 } && _options.MaxSaltLength >= _options.MinSaltLength;


    /// <summary>
    /// Adds an array of randomly generated bytes at the beginning of the
    /// array holding original plain text value.
    /// </summary>
    /// <param name="plainTextBytes">
    /// Byte array containing original plain text value.
    /// </param>
    /// <returns>
    /// Either original array of plain text bytes (if salt is not used) or a
    /// modified array containing a randomly generated salt added at the 
    /// beginning of the plain text bytes. 
    /// </returns>
    private byte[] AddSalt(byte[] plainTextBytes) {
        if (!UseSalt()) return plainTextBytes;
        
        if (_options != null) {
            var saltBytes = GenerateSalt(_options.MinSaltLength, _options.MaxSaltLength);
            
            var plainTextBytesWithSalt = new byte[plainTextBytes.Length + saltBytes.Length];
            Array.Copy(saltBytes, plainTextBytesWithSalt, saltBytes.Length);
            
            Array.Copy(
                plainTextBytes,
                0,
                plainTextBytesWithSalt, 
                saltBytes.Length,
                plainTextBytes.Length
            );

            return plainTextBytesWithSalt;
        }

        return [];
    }

    /// <summary>
    /// Generates an array holding cryptographically strong bytes.
    /// </summary>
    /// <returns>
    /// Array of randomly generated bytes.
    /// </returns>
    /// <remarks>
    /// Salt size will be defined at random or exactly as specified by the
    /// minSlatLen and maxSaltLen parameters passed to the object constructor.
    /// The first four bytes of the salt array will contain the salt length
    /// split into four two-bit pieces.
    /// </remarks>
    private byte[] GenerateSalt(int minSaltLen, int maxSaltLen) {
        
        var saltLen = minSaltLen == maxSaltLen 
            ? minSaltLen 
            : GenerateRandomNumber(minSaltLen, maxSaltLen);
        
        var salt = new byte[saltLen];
        
        using var rng = RandomNumberGenerator.Create();

        rng.GetNonZeroBytes(salt);
        
        salt[0] = (byte)((salt[0] & 0xfc) | (saltLen & 0x03));
        salt[1] = (byte)((salt[1] & 0xf3) | (saltLen & 0x0c));
        salt[2] = (byte)((salt[2] & 0xcf) | (saltLen & 0x30));
        salt[3] = (byte)((salt[3] & 0x3f) | (saltLen & 0xc0));

        return salt;
    }

    /// <summary>
    /// Generates random integer.
    /// </summary>
    /// <param name="minValue">
    /// Min value (inclusive).
    /// </param>
    /// <param name="maxValue">
    /// Max value (inclusive).
    /// </param>
    /// <returns>
    /// Random integer value between the min and max values (inclusive).
    /// </returns>
    /// <remarks>
    /// This methods overcomes the limitations of .NET Framework's Random
    /// class, which - when initialized multiple times within a very short
    /// period of time - can generate the same "random" number.
    /// </remarks>
    private static int GenerateRandomNumber(int minValue, int maxValue) {
        var randomBytes = new byte[4];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        
        var seed = ((randomBytes[0] & 0x7f) << 24) |
                   (randomBytes[1] << 16) |
                   (randomBytes[2] << 8) |
                   randomBytes[3];
        
        var random = new Random(seed);

        // Calculate a random number.
        return random.Next(minValue, maxValue + 1);
    }
    #endregion
}