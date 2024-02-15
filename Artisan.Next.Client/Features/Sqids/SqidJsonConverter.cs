using System.Text.Json;
using System.Text.Json.Serialization;
using Sqids;

namespace Artisan.Next.Client.Features.Sqids;

public class SqidJsonConverter(SqidsEncoder<int> encoder) : JsonConverter<SqidId>
{
    public override SqidId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.GetString() is { } sqid
            ? new SqidId(sqid, encoder)
            : null;

    public override void Write(Utf8JsonWriter writer, SqidId value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Sqid!);
}