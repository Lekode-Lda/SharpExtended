using System.Text.Json;

namespace SharpExtended;

public static class JsonSerializerExtensions {

    /// <summary>
    /// Deserializes an JSON anonymously
    /// </summary>
    /// <param name="json">The string with the JSON</param>
    /// <param name="anonymousTypeObject">The structure of the anonymous object</param>
    /// <param name="options">JsonSerializer options to be apply on deserialization time</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? DeserializeAnonymousType<T>(this string json, T anonymousTypeObject, JsonSerializerOptions? options = default) =>
        JsonSerializer.Deserialize<T>(
            json,
            options ?? new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

    /// <summary>
    /// Extends string to support json deserialization 
    /// </summary>
    /// <param name="json">JSON string to deserialize</param>
    /// <param name="options">JSON Serialization options</param>
    /// <typeparam name="T">Type to deserialize to</typeparam>
    /// <returns>Deserialized JSON</returns>
    public static T? Deserialize<T>(this string json, JsonSerializerOptions? options = default) =>
        JsonSerializer.Deserialize<T>(json, options);

    /// <summary>
    /// Extends objects to support json serialization
    /// </summary>
    /// <param name="json">Class instance to create the JSON</param>
    /// <param name="options">JSON Serialization options</param>
    /// <typeparam name="T">Class type</typeparam>
    /// <returns>Serialized class</returns>
    public static string Serialize<T>(this T json, JsonSerializerOptions? options = default) =>
        JsonSerializer.Serialize(json);

}