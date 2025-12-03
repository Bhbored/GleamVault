using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace GleamVaultApp.Converters
{

    public class SafeEnumConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            // Handle both nullable and non-nullable enums
            var actualType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
            var canConvert = actualType.IsEnum;

            System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] CanConvert called for {typeToConvert.Name}: {canConvert}");

            return canConvert;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var actualType = Nullable.GetUnderlyingType(typeToConvert);

            System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] CreateConverter for {typeToConvert.Name}");

            // If it's nullable, create nullable converter
            if (actualType != null)
            {
                System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] Creating NULLABLE converter for {actualType.Name}");
                var converterType = typeof(SafeEnumConverterNullable<>).MakeGenericType(actualType);
                return (JsonConverter)Activator.CreateInstance(converterType);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] Creating NON-NULLABLE converter for {typeToConvert.Name}");
                var converterType = typeof(SafeEnumConverterInner<>).MakeGenericType(typeToConvert);
                return (JsonConverter)Activator.CreateInstance(converterType);
            }
        }

        // Non-nullable enum converter
        private class SafeEnumConverterInner<T> : JsonConverter<T> where T : struct, Enum
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] Reading non-nullable {typeof(T).Name}");

                if (reader.TokenType == JsonTokenType.String)
                {
                    var stringValue = reader.GetString();
                    System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] String value: '{stringValue}'");

                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        return default;
                    }

                    if (Enum.TryParse<T>(stringValue, ignoreCase: false, out var exactResult))
                    {
                        System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] Exact match: {exactResult}");
                        return exactResult;
                    }

                    if (Enum.TryParse<T>(stringValue, ignoreCase: true, out var caseInsensitiveResult))
                    {
                        System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] Case-insensitive match: {caseInsensitiveResult}");
                        return caseInsensitiveResult;
                    }

                    System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] No match for '{stringValue}', returning default");
                    return default;
                }

                if (reader.TokenType == JsonTokenType.Number)
                {
                    var intValue = reader.GetInt32();
                    if (Enum.IsDefined(typeof(T), intValue))
                    {
                        return (T)(object)intValue;
                    }
                }

                return default;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        // Nullable enum converter
        private class SafeEnumConverterNullable<T> : JsonConverter<T?> where T : struct, Enum
        {
            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] Reading nullable {typeof(T).Name}");

                if (reader.TokenType == JsonTokenType.Null)
                {
                    System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] Value is null");
                    return null;
                }

                if (reader.TokenType == JsonTokenType.String)
                {
                    var stringValue = reader.GetString();
                    System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] String value: '{stringValue}'");

                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        return null;
                    }

                    if (Enum.TryParse<T>(stringValue, ignoreCase: false, out var exactResult))
                    {
                        System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] Exact match: {exactResult}");
                        return exactResult;
                    }

                    if (Enum.TryParse<T>(stringValue, ignoreCase: true, out var caseInsensitiveResult))
                    {
                        System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] Case-insensitive match: {caseInsensitiveResult}");
                        return caseInsensitiveResult;
                    }

                    System.Diagnostics.Debug.WriteLine($"[SafeEnumConverter] No match for '{stringValue}', returning null");
                    return null;
                }

                if (reader.TokenType == JsonTokenType.Number)
                {
                    var intValue = reader.GetInt32();
                    if (Enum.IsDefined(typeof(T), intValue))
                    {
                        return (T)(object)intValue;
                    }
                    return null;
                }

                return null;
            }

            public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                {
                    writer.WriteStringValue(value.Value.ToString());
                }
                else
                {
                    writer.WriteNullValue();
                }
            }
        }
    }
}
