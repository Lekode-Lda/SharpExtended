using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
// ReSharper disable MemberCanBePrivate.Global

namespace SharpExtended;

public static partial class String {

    /// <summary>
    /// Parses a string into bool
    /// </summary>
    /// <param name="value">The string to parse from</param>
    /// <returns>Boolean</returns>
    public static bool ToBool(this string value) =>
        value.Equals("true", StringComparison.CurrentCultureIgnoreCase);

    /// <summary>
    /// Converts from base64 to a string
    /// </summary>
    /// <param name="base64EncodedData">Base64 string</param>
    /// <returns>Plain string</returns>
    public static string Base64Decode(this string base64EncodedData) {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    /// <summary>
    /// Parses a string to a optional GUID
    /// If it's not possible to parse the guid the value will be null
    /// </summary>
    /// <param name="str">String to parse from</param>
    /// <returns>The GUID or null if it's not possible to parse</returns>
    public static Guid? ToGuid(this string str) {
        var parsed = Guid.TryParse(str, out var guid);
        return parsed ? guid : null;
    }
    
    /// <summary>
    /// Alias for Convert.ToUInt64
    /// </summary>
    /// <param name="str">String to convert</param>
    /// <returns>Integer</returns>
    public static ulong ToUlong(this string str) => Convert.ToUInt64(str);

    /// <summary>
    /// Alias for Convert.ToInt64
    /// </summary>
    /// <param name="str">String to convert</param>
    /// <returns>Integer</returns>
    public static long ToLong(this string str) => Convert.ToInt64(str);
    
    /// <summary>
    /// Alias for Convert.ToInt32
    /// </summary>
    /// <param name="str">String to convert</param>
    /// <returns>Integer</returns>
    public static int ToInt32(this string str) => Convert.ToInt32(str);
    

    /// <summary>
    /// Converts a string to base64
    /// </summary>
    /// <param name="plainText">String to convert</param>
    /// <returns>Converted string</returns>
    public static string ToBase64(this string plainText) {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    /// <summary>
    /// Truncates a string to a maximum amount of characters
    /// </summary>
    /// <param name="value">The string to truncate</param>
    /// <param name="maxLength">Maximum amount of characters</param>
    /// <returns>Truncated string</returns>
    public static string Truncate(this string value, int maxLength) {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value[..maxLength];
    }

    /// <summary>
    /// Generates the SHA1 for a string
    /// <see cref="System.Runtime.Intrinsics.Arm.Sha1"/>
    /// </summary>
    /// <param name="input">String to generate the hash for</param>
    /// <returns>Hashed string</returns>
    public static string Sha1(this string input) {
        return Security.Sha1(input);
    }

    /// <summary>
    /// Generates the SHA256 for a string
    /// <see cref="System.Runtime.Intrinsics.Arm.Sha256"/> for the implementation
    /// </summary>
    /// <param name="text">String to generate the hash for</param>
    /// <returns>Hashed string</returns>
    public static string Sha256(this string text) {
        return Security.Sha256(text);
    }

    /// <summary>
    /// Generates the HMAC-SHA256 of a string
    /// <see cref="Security.HmacSha256"/> for the implementation
    /// </summary>
    /// <param name="text">String to generate the HMAC-SHA256 for</param>
    /// <param name="key">The key to generate the signature</param>
    /// <returns>The HMAC-SHA256 in a lowercased hex string</returns>
    public static string HmacSha256(this string text, string key) {
        var hmac = Security.HmacSha256(Encoding.UTF8.GetBytes(text), key);
        return hmac.Aggregate("", (s, e) => s + $"{e:x2}", s => s);
    }

    /// <summary>
    /// Generates the HMAC-MD5 of a string
    /// <see cref="Security.HmacSha256"/> for the implementation
    /// </summary>
    /// <param name="text">String to generate the HMAC-MD5 for</param>
    /// <param name="key">The key to generate the signature</param>
    /// <returns>The HMAC-MD5 in a lowercased hex string</returns>
    public static string HmacMd5(this string text, string key) {
        var hmac = Security.HmacMd5(Encoding.UTF8.GetBytes(text), key);
        return hmac.Aggregate("", (s, e) => s + $"{e:x2}", s => s);
    }

    /// <summary>
    /// Salts and hashes a string
    /// <see cref="Security.GenerateSaltedHash"/> for the implementation
    /// </summary>
    /// <param name="password">String to generate the salted hash, usually a password</param>
    /// <param name="salt">The salt as byte array</param>
    /// <returns>Base64 of the hash</returns>
    public static string SaltAndHash(this string password, string salt) {
        var hash = Security.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt));
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Salts and hashes a string
    /// <see cref="Security.GenerateSaltedHash"/> for the implementation
    /// </summary>
    /// <param name="password">String to generate the salted hash, usually a password</param>
    /// <param name="salt">The salt as string</param>
    /// <returns>Base64 of the hash</returns>
    public static string SaltAndHash(this string password, byte[] salt) {
        var hash = Security.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), salt);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Calculates the MD5 of a string
    /// NOTE: This is not on the Security class because it's no longer a secure hash function 
    /// </summary>
    /// <param name="input">String to generate the hash for</param>
    /// <returns>Hashed string in lowercased hex</returns>
    public static string Md5(this string input) {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        var sb = new StringBuilder();
        foreach (var t in hashBytes) {
            sb.Append(t.ToString("X2"));
        }
        return sb.ToString().ToLower();
    }

    /// <summary>
    /// Checks if a string contains a UNICODE char
    /// </summary>
    /// <param name="input">Input to verify</param>
    /// <returns>Boolean indicating it contains a unicode char or not</returns>
    public static bool ContainsUnicodeCharacter(this string input) {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        const int maxAnsiCode = 255;

        return input.Any(c => c > maxAnsiCode);
    }

    /// <summary>
    /// Verifies if an email is valid
    /// </summary>
    /// <param name="str">String to verify</param>
    /// <returns>Boolean indicating if the string is valid or not</returns>
    public static bool IsValidEmail(this string str) =>
        EmailValidationRegex().IsMatch(str);

    /// <summary>
    /// Compares if two passwords are valid
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <param name="storedHash">Password hash (base64)</param>
    /// <param name="salt">Salt (base64)</param>
    /// <returns>True if the passwords match, false otherwise</returns>
    public static bool ComparePasswords(this string password, string storedHash, string salt) {
        var computedHash = Security.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), Convert.FromBase64String(salt));
        return Security.CompareByteArrays(computedHash, Convert.FromBase64String(storedHash));

    }

    /// <summary>
    /// Gets the string that is closest to another string
    /// </summary>
    /// <param name="text">String to compare</param>
    /// <param name="strings">Array of strings to be compared with</param>
    /// <returns>The string that is closest to the string in the text param</returns>
    public static string ClosestMatch(this string text, string[] strings) {
        var lastFind = "";
        var lastFindDistance = int.MaxValue;
        foreach (var str in strings) {
            var distance = text.ToLower().ComputeLevenshteinDistance(str.ToLower());
            if (lastFindDistance <= distance) continue;
            lastFindDistance = distance;
            lastFind = str;
        }
        return lastFind;
    }

    /// <summary>
    /// Calculates how many transformations are required for the string A to be equal to the string B
    /// </summary>
    /// <param name="s">String to compare with</param>
    /// <param name="t">String to be compared with</param>
    /// <returns>Number of transformations required</returns>
    private static int ComputeLevenshteinDistance(this string s, string t) {
        var n = s.Length;
        var m = t.Length;
        var d = new int[n + 1, m + 1];

        if (n == 0)
            return m;

        if (m == 0)
            return n;

        for (var i = 0; i <= n; d[i, 0] = i++) { }
        for (var j = 0; j <= m; d[0, j] = j++) { }
        for (var i = 1; i <= n; i++) {
            for (var j = 1; j <= m; j++) {
                var cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost
                );
            }
        }

        return d[n, m];
    }
    
    /// <summary>
    /// Generates a CEO friendly string form a string
    /// </summary>
    /// <param name="s">String to convert to CEO string</param>
    /// <returns>Converted string</returns>
    public static string GenerateCeo(this string s) {
        var symbols = new[] { "$", "^", "<", ">", "+", "|", "=" };
        var sb      = new StringBuilder();
        foreach (var c in s.Where(c => !char.IsPunctuation(c))) {
            sb.Append(c);
        }
        foreach (var word in symbols)
            sb.Replace(word, "");
        sb.Replace(" ", "_");
        return sb.ToString();
    }

    /// <summary>
    /// Extension to string.IsNullOrEmpty
    /// </summary>
    /// <param name="str">String to check</param>
    /// <returns>True if the string is null or empty otherwise false</returns>
    public static bool IsNullOrEmpty(this string? str) => string.IsNullOrEmpty(str);

    /// <summary>
    /// Truncates a chunk of HTML by the Inner Text of the document but keeps the HTML tags
    /// Unclosed tags will be automatically closed
    /// This function uses the <see cref="TruncateWords"/> method
    /// </summary>
    /// <param name="html">HTML to truncate</param>
    /// <param name="charLimit">Maximum amount of characters</param>
    /// <param name="insertDots">If the text was truncated auto insert "..." before closing the last tag</param>
    /// <returns>The truncated HTML</returns>
    public static string TruncateHtmlByWords(this string html, int charLimit, bool insertDots = true) {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var tagMatches   = Regex.Matches(doc.DocumentNode.OuterHtml, "<.*?>");
        var tagPositions = new Dictionary<int, (string, bool)>();

        foreach (Match tag in tagMatches)
            tagPositions[tag.Index] = (tag.Value, tag.Value.Contains('/'));

        var unclosedStack   = new Stack<string>();
        var truncatedText   = doc.DocumentNode.InnerText.TruncateWords(charLimit, out var truncated);

        foreach (var tagPosition in tagPositions.Where(tagPosition => tagPosition.Key < truncatedText.Length)) {
            if (tagPosition.Value.Item2)
                _ = unclosedStack.TryPop(out _);
            if (tagPosition.Value.Item1 != "<br>" && !tagPosition.Value.Item2)
                unclosedStack.Push(tagPosition.Value.Item1);
            truncatedText = truncatedText.Insert(tagPosition.Key, tagPosition.Value.Item1);
        }

        for (var i = 0; i <= unclosedStack.Count; i++) {
            if (unclosedStack.Count == 1 && insertDots && truncated)
                truncatedText += "...";

            var success = unclosedStack.TryPop(out var close);
            if (success)
                truncatedText += close?.Replace("<", "</");
        }

        return truncatedText;

    }

    /// <summary>
    /// Truncates text to a maximum amount of characters, this will not split words
    /// </summary>
    /// <param name="text">Text to truncate</param>
    /// <param name="maxLength">Maximum amount of characters</param>
    /// <param name="truncated">Specifies if the string was truncated or not</param>
    /// <returns>The truncated string or the entire string of the maxLength is higher than the text length</returns>
    public static string TruncateWords(this string text, int maxLength, out bool truncated) {
        if (text.Length <= maxLength) {
            truncated = false;
            return text;
        }

        var lastSpaceIndex = text[..maxLength].LastIndexOf(' ');
        var truncatedText  = text[..lastSpaceIndex];
        truncated = true;
        return truncatedText;
    }

    [GeneratedRegex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")]
    private static partial Regex EmailValidationRegex();

    /// <summary>
    /// Downloads an image (async) and saves it
    /// </summary>
    /// <param name="url">URL of the image</param>
    /// <param name="filePath">Path to save the image</param>
    public static async Task DownloadImage(this string url, string filePath) {
        using var client   = new HttpClient();
        var       response = await client.GetAsync(new Uri(url));
        var       content  = await response.Content.ReadAsByteArrayAsync();
        await File.WriteAllBytesAsync(filePath, content);
    }

    /// <summary>
    /// Downloads an image (async) and stores it in memory
    /// </summary>
    /// <param name="url">URL of the image</param>
    public static async Task<byte[]> DownloadImage(this string url) {
        using var client   = new HttpClient();
        var       response = await client.GetAsync(new Uri(url));
        var       content  = await response.Content.ReadAsByteArrayAsync();
        return content;
    }
    
}
