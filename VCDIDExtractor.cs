using System;
using System.IO;
using System.Text.RegularExpressions;

// This code is a C# rewrite (by Github CoPilot) of the C++ code 
// by israpps from https://github.com/israpps/POPS-VCD-ID-Extractor?tab=readme-ov-file

namespace PopsVcdIdExtractor
{
    public static class VCDIdExtractor
    {
        private const int ELF_SEARCH_RANGE = 4096;
        private const int ELF_SEARCH_OFFSET = 1100032;
        private static readonly Regex ElfIdRegex = new Regex(@"[A-Z]{4}[_-][0-9]{3}\.[0-9]{2}", RegexOptions.Compiled);

        public enum ExtractResult
        {
            Success,
            FileNotFound,
            FileTooSmall,
            RegexNotFound,
            IOError
        }

        public static ExtractResult TryExtractVCDId(string filePath, out string vcdId)
        {
            vcdId = null;

            if (!File.Exists(filePath))
                return ExtractResult.FileNotFound;

            FileInfo fi = new FileInfo(filePath);
            if (fi.Length < ELF_SEARCH_OFFSET + ELF_SEARCH_RANGE)
                return ExtractResult.FileTooSmall;

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(ELF_SEARCH_OFFSET, SeekOrigin.Begin);
                    byte[] buffer = new byte[ELF_SEARCH_RANGE];
                    int bytesRead = fs.Read(buffer, 0, ELF_SEARCH_RANGE);

                    // Replace non-printable characters with ','
                    for (int i = 0; i < bytesRead; i++)
                        if (buffer[i] < 32)
                            buffer[i] = (byte)',';

                    string str = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    Match match = ElfIdRegex.Match(str);
                    if (match.Success)
                    {
                        vcdId = match.Value;
                        return ExtractResult.Success;
                    }
                    else
                    {
                        return ExtractResult.RegexNotFound;
                    }
                }
            }
            catch (IOException)
            {
                return ExtractResult.IOError;
            }
        }
    }
}