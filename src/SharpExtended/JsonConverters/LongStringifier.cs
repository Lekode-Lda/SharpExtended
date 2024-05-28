using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharpExtended.JsonConverters;

public class LongStringifier : JsonConverter<long> {

    /// <inheritdoc/>
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        return reader.TokenType switch {
            JsonTokenType.Number => reader.GetInt64(),
            JsonTokenType.String => Convert.ToInt64(reader.GetString()),
            _                    => throw new JsonException()
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }

}
