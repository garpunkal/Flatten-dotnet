using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .Build();

var destination = new DirectoryInfo(Directory.GetCurrentDirectory());

FlattenFiles(destination, destination);
CleanUp(destination, config);

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("fin.");
Console.ResetColor();

static void CleanUp(DirectoryInfo root, IConfiguration config)
{
    var extensions = config["FileExtensionsToBeRemoved"]
        ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        ?? [];

    foreach (var file in GetFiles(root).Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase)))
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(file.FullName);
        Console.ResetColor();

        try
        {
            File.Delete(file.FullName);
        }
        catch (Exception ex)
        {
            WriteError(ex.Message);
        }
    }
}

static void FlattenFiles(DirectoryInfo root, DirectoryInfo destination)
{
    if (root.FullName != destination.FullName)
    {
        foreach (var file in GetFiles(root))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(file.FullName);
            Console.ResetColor();

            var target = Path.Combine(destination.FullName, file.Name);

            try
            {
                if (File.Exists(target))
                    File.Delete(target);

                File.Move(file.FullName, target);
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }
        }
    }

    foreach (var dir in root.GetDirectories())
        FlattenFiles(dir, destination);

    if (root.FullName != destination.FullName)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(root.FullName);
        Console.ResetColor();

        try
        {
            root.Delete();
        }
        catch (Exception ex)
        {
            WriteError(ex.Message);
        }
    }
}

static IEnumerable<FileInfo> GetFiles(DirectoryInfo root, string filter = "*.*")
{
    try
    {
        return root.GetFiles(filter);
    }
    catch (UnauthorizedAccessException ex)
    {
        WriteError(ex.Message);
        return [];
    }
    catch (DirectoryNotFoundException ex)
    {
        WriteError(ex.Message);
        return [];
    }
}

static void WriteError(string message)
{
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine(message);
    Console.ResetColor();
}
