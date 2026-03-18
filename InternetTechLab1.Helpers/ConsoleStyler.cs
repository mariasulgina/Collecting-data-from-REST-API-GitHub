namespace InternetTechLab1.Helpers;

public static class ConsoleStyler
{
    public const string Bold = "\x1b[1m";
    public const string Reset = "\x1b[0m";
    
    public static void PrintHeader(string title)
    {
        Console.WriteLine($"\n{Bold}{Cyan}---- {title.ToUpper()} ----{Reset}");
    }
}
