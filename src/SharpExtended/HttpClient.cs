using System.Net.Http.Formatting;
using Vima.Ensure.Net;

namespace SharpExtended;

public static class HttpClientExtension {
    
    /// <summary>
    /// Allows to create a PATCH request as JSON (async)
    /// </summary>
    /// <param name="client">HTTPClient that will be performing the request</param>
    /// <param name="requestUri">The URL of the request</param>
    /// <param name="value">The data to be sent</param>
    /// <typeparam name="T">Type of the data to be sent</typeparam>
    /// <returns>The result of the request</returns>
    public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value) {
        Ensure.NotNull(client, "client");
        Ensure.NotNullOrEmpty(requestUri, "requestUri");

        var content = new ObjectContent<T>(value, new JsonMediaTypeFormatter());
        var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = content };

        return client.SendAsync(request);
    }
}