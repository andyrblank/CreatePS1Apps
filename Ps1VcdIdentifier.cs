using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PopsVcdIdExtractor;

public static class PS1VCDIdentifier
{
    public static void IdentifyAndExportToCsv(string folderPath, string psxDataJsonPath)
    {
        var vcdFiles = Directory.GetFiles(folderPath, "*.vcd", SearchOption.TopDirectoryOnly);
        if (vcdFiles.Length == 0)
        {
            Console.WriteLine("No .vcd files found in the specified folder.");
            return;
        }

        var psxGameDict = PS1GameInfo.Load(psxDataJsonPath);

        var results = new List<(string Id, string Title)>();
        var logLines = new List<string>();

        foreach (var vcdFile in vcdFiles)
        {
            if (VCDIdExtractor.TryExtractVCDId(vcdFile, out var vcdId) == VCDIdExtractor.ExtractResult.Success && !string.IsNullOrWhiteSpace(vcdId))
            {
                var idKey = vcdId.Replace(".", "").Replace("_", "-");
                var match = psxGameDict.Keys.FirstOrDefault(k => string.Equals(k.Replace(".", ""), idKey, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    var title = psxGameDict[match]?.Title;
                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        var output = $"ID: {idKey} Title: {title}";
                        Console.WriteLine(output);
                        logLines.Add(output);
                        results.Add((idKey, title));
                    }
                    else
                    {
                        var output = $"ID: {idKey} Title: Unable to find a Title for this ID in the database.";
                        Console.WriteLine(output);
                        logLines.Add(output);
                        results.Add((idKey, ""));
                    }
                }
                else
                {
                    var output = $"ID: {idKey} Title: Unable to find a Title for this ID in the database.";
                    Console.WriteLine(output);
                    logLines.Add(output);
                    results.Add((idKey, "Unidentified"));
                }
            }
            else
            {
                var output = $"Unable to find an ID in VCD File \"{Path.GetFileName(vcdFile)}\"";
                Console.WriteLine(output);
                logLines.Add(output);
                results.Add((Path.GetFileNameWithoutExtension(vcdFile), "Unidentified"));
            }
        }

        var csvPath = Path.Combine(folderPath, "ps1_vcd_identified.csv");
        using (var writer = new StreamWriter(csvPath, false))
        {
            writer.WriteLine("id,title");
            foreach (var (id, title) in results)
            {
                writer.WriteLine($"{id},{EscapeCsv(title)}");
            }
        }

        var logPath = Path.Combine(folderPath, "ps1_vcd_identified_log.txt");
        File.WriteAllLines(logPath, logLines);

        Console.WriteLine($"CSV output written to: {csvPath}");
        Console.WriteLine($"Log output written to: {logPath}");
    }

    private static string EscapeCsv(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";
        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }
}