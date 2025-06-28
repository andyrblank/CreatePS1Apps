using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public static class OplAppCreator
{
    public static void CreateApps(
        string oplFolder,
        List<OPLGameList> games,
        bool testMode,
        string logFile,
        string popstarterElf)
    {
        var idTitleMap = games
            .Where(g => !string.IsNullOrWhiteSpace(g.ID) && !string.IsNullOrWhiteSpace(g.Title))
            .Select(g => new { g.ID, g.Title })
            .ToList();

        var popsDir = Path.Combine(oplFolder, "POPS");
        var vcdFiles = Directory.GetFiles(popsDir, "*.vcd", SearchOption.TopDirectoryOnly);
        var logLines = new List<string>();

        foreach (var vcdPath in vcdFiles)
        {
            var vcdName = Path.GetFileNameWithoutExtension(vcdPath);
            var id = GetIdFromVcdName(vcdName);
            if (id != null && idTitleMap.Where(i => i.ID == id).Any())
            {
                var title = idTitleMap.First(i => i.ID == id).Title;
                var sanitizedTitle = SanitizeTitle(title);
                var appFolder = Path.Combine(oplFolder, "APPS", $"PS1_{sanitizedTitle}");
                var elfName = $"XX.{id}.{sanitizedTitle}.ELF";
                var cfgPath = Path.Combine(appFolder, "title.cfg");
                var elfDest = Path.Combine(appFolder, elfName);

                if (testMode)
                {
                    logLines.Add($"Would create folder: {appFolder}");
                    logLines.Add($"Would write file: {cfgPath} with content:");
                    logLines.Add($"  title=[PS1] {sanitizedTitle}");
                    logLines.Add($"  boot=XX.{id}.{sanitizedTitle}.ELF");
                    logLines.Add($"Would copy and rename POPSTARTER.ELF to {elfDest}");
                    logLines.Add($"Would create app for {sanitizedTitle} ({id})");
                }
                else
                {
                    Directory.CreateDirectory(appFolder);
                    File.WriteAllLines(cfgPath, new[] {
                        $"title=[PS1] {sanitizedTitle}",
                        $"boot=XX.{id}.{sanitizedTitle}.ELF"
                    });
                    File.Copy(popstarterElf, elfDest, true);
                    Console.WriteLine($"Created app for {sanitizedTitle} ({id})");
                }
            }
            else
            {
                if (testMode)
                    logLines.Add($"Would skip {Path.GetFileName(vcdPath)} (ID not found in list.csv)");
                else
                    Console.WriteLine($"Skipping {Path.GetFileName(vcdPath)} (ID not found in list.csv)");
            }
        }

        if (testMode)
        {
            File.AppendAllLines(logFile, logLines);
            Console.WriteLine($"[TEST MODE] Actions written to \"{logFile}\"");
            Console.WriteLine($"Done. See \"{logFile}\" for details.");
        }
        else
        {
            Console.WriteLine("Done.");
        }
    }

    private static string GetIdFromVcdName(string vcdName)
    {
        var parts = vcdName.Split('.');
        if (parts.Length >= 2)
            return $"{parts[0]}.{parts[1]}";
        return null;
    }

    private static string SanitizeTitle(string title)
    {
        var invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        var regex = new Regex($"[{Regex.Escape(invalidChars)}]");
        return regex.Replace(title, "");
    }
}