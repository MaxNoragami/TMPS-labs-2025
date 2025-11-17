namespace lab2.Utils;

public static class Input
{
    public static string ReadNonEmpty(string prompt)
    {
        string? input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                Console.WriteLine("Input cannot be empty!");
        } while (string.IsNullOrWhiteSpace(input));
        return input;
    }

    public static int ReadMenuChoice(string prompt, params string[] options)
    {
        int choice;
        do
        {
            Console.WriteLine(prompt);
            for (int i = 0; i < options.Length; i++)
                Console.WriteLine($"{i + 1}: {options[i]}");
            Console.Write("> ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out choice) && choice >= 1 && choice <= options.Length)
                return choice;
            Console.WriteLine("Invalid choice. Try again.");
        } while (true);
    }

    public static string ReadFormat()
    {
        string fmt;
        do
        {
            fmt = ReadNonEmpty("Enter target format (e.g. mp4): ");
            if (fmt.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || fmt.Length < 2)
                Console.WriteLine("Invalid format. Try again.");
            else
                break;
        } while (true);
        return fmt;
    }

    public static string ReadWatermarkPath()
    {
        string wm;
        do
        {
            wm = ReadNonEmpty("Enter watermark image path: ");
            if (!File.Exists(wm))
                Console.WriteLine("File does not exist. Try again.");
            else
                break;
        } while (true);
        return wm;
    }

    public static string ReadResolution()
    {
        string res;
        do
        {
            res = ReadNonEmpty("Enter resolution (e.g. 1920x1080): ");
            var parts = res.Split('x');
            if (parts.Length != 2 || !int.TryParse(parts[0], out var w) || !int.TryParse(parts[1], out var h) || w <= 0 || h <= 0)
                Console.WriteLine("Invalid resolution. Try again.");
            else
                break;
        } while (true);
        return res;
    }

    public static bool ReadYesNo(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = (Console.ReadLine() ?? "").Trim().ToLower();
            if (input == "y" || input == "yes") return true;
            if (input == "n" || input == "no") return false;
            Console.WriteLine("Please enter 'y' or 'n'.");
        }
    }

    public static string GetVideoOutputFile(string? extension = null)
    {
        string file;
        do
        {
            var ext = extension ?? "mp4";
            file = ReadNonEmpty($"Enter output file (will be saved as .{ext}): ");
            file = Path.ChangeExtension(file, $".{ext}");
            if (file.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                Console.WriteLine("Invalid file name. Try again.");
            else
                break;
        } while (true);
        return file;
    }


    public static string GetMetadataOutputFile()
    {
        string file;
        do
        {
            file = ReadNonEmpty("Enter metadata output file (will be saved as .json): ");
            file = Path.ChangeExtension(file, ".json");
            if (file.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                Console.WriteLine("Invalid file name. Try again.");
            else
                break;
        } while (true);
        return file;
    }
}