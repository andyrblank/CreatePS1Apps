using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public static class PS1GameInfo
{
    /// <summary>
    /// Loads the PSX.data.json file and returns a dictionary mapping game keys to PsxGameInfo objects.
    /// Data source: https://github.com/niemasd/GameDB-PSX
    /// </summary>
    /// <param name="jsonPath">The path to PSX.data.json</param>
    /// <returns>Dictionary of game key to PsxGameInfo</returns>
    public static Dictionary<string, PsxGameInfo> Load(string jsonPath)
    {
        if (!File.Exists(jsonPath))
            throw new FileNotFoundException("PSX.data.json not found.", jsonPath);

        var json = File.ReadAllText(jsonPath);

        var settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        return JsonConvert.DeserializeObject<Dictionary<string, PsxGameInfo>>(json, settings)
               ?? new Dictionary<string, PsxGameInfo>();
    }
}