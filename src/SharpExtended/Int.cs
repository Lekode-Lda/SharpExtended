using System.Security.Cryptography;

namespace SharpExtended;

public static class IntExtension {

    /// <summary>
    /// Generates a random number cryptographically 
    /// </summary>
    /// <param name="min">Minimum number that can be generated. Defaults to 0</param>
    /// <param name="max">Maximum number that can be generated. Defaults to 10</param>
    /// <returns></returns>
    public static int CryptoRandom(int min = 0, int max = 10) {
        using var rand = RandomNumberGenerator.Create();
        var scale = uint.MaxValue;
        while (scale == uint.MaxValue) {
            var fourBytes = new byte[4];
            rand.GetBytes(fourBytes);
            scale = BitConverter.ToUInt32(fourBytes, 0);
        }
        return (int)(min + (max - min) * (scale / (double)uint.MaxValue));
    }

}