using System;
using System.IO;

class OPLPS1AppUtility
{
    static void Main()
    {
        // Menu
        int mode = 0;        
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1. Test Creating PS1 Apps and output actions to Create_PS1_Apps_Test.log");
            Console.WriteLine("2. Create PS1 Apps (real run)");
            Console.WriteLine("3. Identify PS1 VCD files in Folder and output to CSV");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option (1-4): ");
            var choice = Console.ReadLine();
            if (choice == "1") { mode = 1; break; }
            if (choice == "2") { mode = 2; break; }
            if (choice == "3") { mode = 3; break; }
            if (choice == "4") { return; }
            Console.WriteLine("Invalid option.");
        }
        if(mode == 1 || mode == 2)
        {
            Console.Write("Enter the full path to your OPL folder: ");
            var oplFolder = Console.ReadLine()?.Trim('"');
            if (string.IsNullOrWhiteSpace(oplFolder))
            {
                Console.WriteLine("No path entered. Exiting.");
                Pause();
                return;
            }

            // Validate folder structure
            if (!Directory.Exists(Path.Combine(oplFolder, "APPS")) ||
                !Directory.Exists(Path.Combine(oplFolder, "POPS")) ||
                !File.Exists(Path.Combine(oplFolder, "list.csv")) ||
                !File.Exists(Path.Combine(oplFolder, "POPS", "POPSTARTER.ELF")))
            {
                Console.WriteLine("Required folders/files not found. Exiting.");
                Pause();
                return;
            }
            string logFile = Path.Combine(oplFolder, "Create_PS1_Apps_Test.log");
            if (mode == 1 && File.Exists(logFile))
                File.Delete(logFile);

            // Load games from list.csv
            var games = OplGameListLoader.Load(Path.Combine(oplFolder, "list.csv"));

            // Create OPL Apps for PS1 games
            OplAppCreator.CreateApps(
                oplFolder,
                games,
                mode == 1,
                logFile,
                Path.Combine(oplFolder, "POPS", "POPSTARTER.ELF")
            );
        }
        

        if (mode == 3)
        {
            Console.Write("Enter the path to the folder containing .vcd files: ");
            var vcdFolder = Console.ReadLine()?.Trim('"');
            if (string.IsNullOrWhiteSpace(vcdFolder) || !Directory.Exists(vcdFolder))
            {
                Console.WriteLine("Invalid folder path.");
                Pause();
                return;
            }
            PS1VCDIdentifier.IdentifyAndExportToCsv(vcdFolder, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "PSX.data.json"));
            Pause();
            return;
        }
        
        Pause();
    }

    static void Pause()
    {
        Console.WriteLine("Press any key to continue . . .");
        Console.ReadKey(true);
    }
}