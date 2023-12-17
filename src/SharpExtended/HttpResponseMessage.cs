using System.Text.Json;

namespace SharpExtended;

public static class HttpResponseMessageExtension {

    /// <summary>
    /// Adds the capability of reading a response directly to a Model
    /// </summary>
    /// <param name="content">Content of the response</param>
    /// <param name="options">JSON Deserialization options</param>
    /// <typeparam name="T">Type to deserialize to</typeparam>
    /// <returns>Deserialized response, if an error occurs returns null</returns>
    public static async Task<T?> ReadJsonContentAsAsync<T>(
        this HttpContent       content, 
        JsonSerializerOptions? options = null
    ) => 
        JsonSerializer.Deserialize<T>(await content.ReadAsStringAsync(), options);

}