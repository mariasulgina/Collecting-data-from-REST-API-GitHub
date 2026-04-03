namespace InternetTechLab1.UI;

/// <summary>
/// Статический вспомогательный класс для стилизации вывода в консоль.
/// </summary>
public static class ConsoleStyler
{
    public const string Bold = "\x1b[1m";
    public const string Reset = "\x1b[0m";
    public const string Cyan = "\x1b[36m";
    
    /// <summary>
    /// Выводит в консоль стилизованный заголовок секции.
    /// </summary>
    public static void PrintHeader(string title)
    {
        Console.WriteLine($"\n{Bold}{Cyan}---- {title.ToUpper()} ----{Reset}");
        Console.WriteLine(new string('-', 50));
    }

    /// <summary>
    /// Выводит в консоль горизонтальную линию-разделитель.
    /// </summary>
    public static void PrintIndentation()
    {
        Console.WriteLine("\n" + new string('-', 50));
    }
}
