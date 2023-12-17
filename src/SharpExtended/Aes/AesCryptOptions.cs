using System.Security.Cryptography;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace SharpExtended.Aes;

public class AesCryptOptions {

    #region Properties
    /// <summary>
    /// Key Size: this is typically 128, 192 or 256, depending on the password length in bit (16, 24 or 32 respectively).
    /// Default is NULL, meaning that it will be calculated on-the-fly using the password bit length. 
    /// </summary>
    public int? FixedKeySize { get; set; } = null;

    /// <summary>
    /// Password hashing mode: None, MD5 or SHA1.
    /// SHA1 is the recommended mode for most scenarios.
    /// </summary>
    public AesPasswordHash PasswordHash { get; set; } = AesPasswordHash.Sha1;

    /// <summary>
    /// Password iterations - not used when [PasswordHash] is set to [AESPasswordHash.None]
    /// </summary>
    public int PasswordHashIterations { get; set; } = 1;

    /// <summary>
    ///  Minimum Salt Length: must be equal or smaller than MaxSaltLength.
    ///  Default is 0.
    /// </summary>
    public int MinSaltLength { get; set; } = 0;

    /// <summary>
    ///  Maximum Salt Length: must be equal or greater than MinSaltLength.
    ///  Default is 0, meaning that no salt will be used.
    /// </summary>
    public int MaxSaltLength { get; set; } = 0;

    /// <summary>
    /// Salt value used for password hashing during key generation.
    /// NOTE: This is not the same as the salt we will use during encryption.
    /// This parameter can be any string (set it to NULL for no password hash salt): default is NULL.
    /// </summary>
    public string PasswordHashSalt { get; set; } = "";

    /// <summary>
    /// Sets the Padding Mode (default is PaddingMode.PKCS7)
    /// </summary>
    public PaddingMode PaddingMode { get; set; } = PaddingMode.PKCS7;

    #endregion

}