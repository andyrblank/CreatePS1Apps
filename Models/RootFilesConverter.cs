using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class RootFilesConverter : JsonConverter<List<string>>
{
    public override List<string>? ReadJson(JsonReader reader, Type objectType, List<string>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            return new List<string> { (string)reader.Value! };
        }
        else if (reader.TokenType == JsonToken.StartArray)
        {
            var array = JArray.Load(reader);
            var list = new List<string>();
            foreach (var item in array)
            {
                if (item.Type == JTokenType.String)
                    list.Add(item.ToString());
            }
            return list;
        }
        return null;
    }

    public override void WriteJson(JsonWriter writer, List<string>? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        if (value.Count == 1)
            writer.WriteValue(value[0]);
        else
        {
            writer.WriteStartArray();
            foreach (var item in value)
                writer.WriteValue(item);
            writer.WriteEndArray();
        }
    }
}