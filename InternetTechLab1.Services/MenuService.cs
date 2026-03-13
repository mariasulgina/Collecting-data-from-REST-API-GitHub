using System;
using static System.Console;
using InternetTechLab1.Data.Rdbms;

namespace InternetTechLab1.Services;

public class MenuService
{
    private readonly List<string> _mainMenu;
    private readonly List<string>[] _subMenus;
    private readonly Dictionary<(int main, int sub), Func<ICommand>> _menuToCommandFactory;

    public MenuService()
    {
        _mainMenu = new List<string> 
        { 
            "Модуль A — Получение данных через API → сохранение в реляционную БД", 
            "Модуль B — Web Scraping по URL → сохранение в документоориентированную БД", 
            "Выход"
        };
        
        _subMenus = new[] 
        {
            new List<string> { "Получить данные о пользователе", "Репозитории", "Подписчики пользователя", "Текущий пользователь", "Посмотреть базу данных", "Очистить базу данных", "Выход" },
            new List<string> { "Web Scraping по URL", "Выход" }
        };

        _menuToCommandFactory = new Dictionary<(int main, int sub), Func<ICommand>>
        {
            { (0, 0), () => new GetUserCommand(CreateGitHubService()) },
            { (0, 1), () => new GetReposCommand(CreateGitHubService()) },
            { (0, 2), () => new GetFollowersCommand(CreateGitHubService()) },
            { (0, 3), () => new GetCurrentUserCommand(CreateGitHubService()) },
            { (0, 4), () => new ShowDbCommand(CreateSqliteService()) },
            { (0, 5), () => new ClearDbCommand(CreateSqliteService()) },

            { (1, 0), () => new WebScrapingCommand() },
        };
    }

    public void Run() 
    {
        while (true) 
        {
            int mainChoice = ShowMenu(_mainMenu);
            if (mainChoice == _mainMenu.Count - 1) return;
            Console.Clear();

            int subChoice = ShowMenu(_subMenus[mainChoice]);

            var key = (mainChoice, subChoice);
            if (_menuToCommand.TryGetValue(key, out var command))
            {
                Clear();
                command.Execute();
            }
        }
    }

    private int ShowMenu(List<string> stringItems)
    {
        foreach (var item in stringItems) 
        {
            Console.WriteLine(item);
        }

        int currentPosition = 0;
        int startRow = Console.CursorTop;
        int startCol = Console.CursorLeft;
        ConsoleKeyInfo cki;

        do 
        {
            Console.SetCursorPosition(startCol, startRow);
            Console.Write(new string(' ', 100));
            Console.SetCursorPosition(startCol, startRow);
            Console.Write($"-> {stringItems[currentPosition]}");
            cki = Console.ReadKey();

            switch(cki.Key)
            {
                case ConsoleKey.UpArrow:
                    currentPosition = Math.Max(currentPosition - 1, 0);
                    break;
                case ConsoleKey.DownArrow:
                    currentPosition = Math.Min(currentPosition + 1, stringItems.Count - 1); 
                    break;
                case ConsoleKey.Enter:
                    Console.WriteLine($"Выбрано: {stringItems[currentPosition]}");
                    return currentPosition;
            }
        } while (true);
    }
}
