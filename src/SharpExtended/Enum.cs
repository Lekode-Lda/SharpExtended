using System.ComponentModel;

namespace SharpExtended;

/// <summary>
/// Exception when the Enum can't be parsed
/// </summary>
public class InvalidEnumException : Exception {

    /// <summary>
    /// Default initializer, for message only
    /// </summary>
    /// <param name="message">Error message</param>
    public InvalidEnumException(string message) : base(message) { }
}

/// <summary>
/// Adds extensions to classes
/// </summary>
public static class EnumExtensions {        

    /// <summary>
    /// Parses a string to a enum
    /// If a num is not found throws InvalidEnumException
    /// </summary>
    /// <typeparam name="T">The type of the enum</typeparam>
    /// <param name="str">The string to be parsed</param>
    /// <returns>The enum value</returns>
    public static T ParseEnum<T>(this string str) where T : Enum {
        foreach (T e in Enum.GetValues(typeof(T))) {
            if (e.GetDescription() == str)
                return e;
        }
        throw new InvalidEnumException("The given enum type doesn't contain " + str);
    }

    /// <summary>
    /// Attempts to parse a string to a enum
    /// </summary>
    /// <param name="str">The string to be parsed</param>
    /// <param name="enum">The out value of the parsed enum from the input string</param>
    /// <typeparam name="T">The type of the enu</typeparam>
    /// <returns>The result of the parse, true if successfully parsed false otherwise</returns>
    public static bool TryParseEnum<T>(this string str, out T? @enum) where T : Enum {
        foreach (T? e in Enum.GetValues(typeof(T))) {
            if (e.GetDescription() != str) continue;
            @enum = e;
            return true;
        }

        @enum = default;
        return false;
    }

    /// <summary>
    /// Gets the description of the enumerator
    /// </summary>
    /// <param name="e">The enumerator</param>
    /// <returns>The description as string</returns>
    public static string? GetDescription(this Enum? e) {
        var eType = e?.GetType();
        if (eType == null || e == null)
            return null;
        
        var eName = Enum.GetName(eType, e);
        if (eName == null)
            return null;
        
        var fieldInfo = eType.GetField(eName);
        if (fieldInfo == null)
            return null;
        
        var descriptionAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return descriptionAttribute?.Description;
    }
    
}