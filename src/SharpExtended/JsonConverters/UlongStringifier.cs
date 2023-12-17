using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharpExtended.JsonConverters;

public class UlongStringifier : JsonConverter<ulong> {
    
    /// <inheritdoc/>
    public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        return reader.TokenType switch {
            JsonTokenType.Number => reader.GetUInt64(),
            JsonTokenType.String => Convert.ToUInt64(reader.GetString()),
            _                    => throw new JsonException()
        };
    }
    
    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }
    
}