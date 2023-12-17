using System.Collections.Specialized;

namespace SharpExtended;

public static class NameValueCollectionExtensions {

    /// <summary>
    /// Converts a NameValueCollection to a Dictionary
    /// </summary>
    /// <param name="collection">NameValueCollection to convert to a Dictionary</param>
    /// <returns>Dictionary of string, string</returns>
    public static Dictionary<string, string?> ToDictionary(this NameValueCollection collection) =>
        collection.AllKeys.ToDictionary(key => key ?? "", key => collection[key]);

}