using System.Collections.Generic;
using Newtonsoft.Json;

public class PsxGameInfo
{
    [JsonProperty("redump_name")]
    public string? RedumpName { get; set; }

    [JsonProperty("root_files")]
    [JsonConverter(typeof(RootFilesConverter))]
    public List<string>? RootFiles { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("uuid")]
    public string? Uuid { get; set; }

    [JsonProperty("volume_ID")]
    public string? VolumeId { get; set; }

    [JsonProperty("developer")]
    [JsonConverter(typeof(StringOrStringArrayConverter))]
    public List<string>? Developer { get; set; }

    [JsonProperty("genre")]
    [JsonConverter(typeof(StringOrStringArrayConverter))]
    public List<string>? Genre { get; set; }

    [JsonProperty("language")]
    [JsonConverter(typeof(StringOrStringArrayConverter))]
    public List<string>? Language { get; set; }

    [JsonProperty("publisher")]
    [JsonConverter(typeof(StringOrStringArrayConverter))]
    public List<string>? Publisher { get; set; }

    [JsonProperty("region")]
    public string? Region { get; set; }

    [JsonProperty("release_date")]
    public string? ReleaseDate { get; set; }
}