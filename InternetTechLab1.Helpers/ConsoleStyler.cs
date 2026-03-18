namespace InternetTechLab1.Helpers;

public static class ConsoleStyler
{
    public const string Bold = "\x1b[1m";
    public const string Reset = "\x1b[0m";
    public const string Cyan = "\x1b[36m";
    
    public static void PrintHeader(string title)
    {
        Console.WriteLine("\n" + new string('-', 50));
        Console.WriteLine($"\n{Bold}{Cyan}---- {title.ToUpper()} ----{Reset}");
        Console.WriteLine(new string('-', 50));
    }

    public static void PrintIndentation()
    {
        Console.WriteLine("\n" + new string('-', 50));
    }
}
