namespace SharpExtended.Rsa;

public class KeyPair(string publicKey, string privateKey) {
    public string Public  { get; } = publicKey;
    public string Private { get; } = privateKey;
}
