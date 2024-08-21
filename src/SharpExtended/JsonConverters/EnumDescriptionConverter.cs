using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
// ReSharper disable NullableWarningSuppressionIsUsed
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace SharpExtended.JsonConverters;

public class EnumDescriptionConverter : JsonConverter<object> {
    
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    /// <inheritdoc />
    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        GetEnumValue(typeToConvert, reader.GetString() ?? "");

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) {
        if (value is Enum) {
            var fi         = value.GetType().GetField(value.ToString() ?? "");
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            writer.WriteStringValue(attributes.Length > 0 ? attributes[0].Description : value.ToString());
        } else
            JsonSerializer.Serialize(writer, value, options);
    }

    /// <summary>
    /// Gets the description of the enum
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static object GetEnumValue(Type type, string value) {
        if (Nullable.GetUnderlyingType(type) != null) {
            type = Nullable.GetUnderlyingType(type)!;
        }

        foreach (var name in Enum.GetNames(type))
        {
            var fi         = type.GetField(name);
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0 && attributes[0].Description == value)
                return Enum.Parse(type, name);
        }
        return Enum.Parse(type, value);
    }
}
