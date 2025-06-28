using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public static class OplGameListLoader
{
    public static List<OPLGameList> Load(string csvPath)
    {
        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            IgnoreBlankLines = true,            
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null
        });
        
        csv.Read();
        csv.Read(); // skip "sep=,"
        csv.ReadHeader();

        return csv.GetRecords<OPLGameList>().ToList();
    }
}