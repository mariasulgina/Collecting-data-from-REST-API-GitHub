using System;
using static System.Console;
using InternetTechLab1.Data.Rdbms;

namespace InternetTechLab1.Services;

public class MenuService
{
    private readonly List<string> _mainMenu = new List<string> {"Модуль A — Получение данных через API → сохранение в реляционную БД", "Модуль B — Web Scraping по URL → сохранение в документоориентированную БД", "Выход"};
    private readonly List<string>[] _subMenus = 
    {
        new List<string> {"Получить данные о пользователе", "Репозитории", "Подписчики пользователя", "Текущий пользователь", "Посмотреть базу данных", "Очистить базу данных", "Выход"},
        new List<string> {"Web Scraping по URL ", "Выход"}
    };
    private IGitHubApiService CreateGitHubService() => new GitHubApiService();
    private ICommand CreateCommandService() => new ;
    private IDatabaseService CreateSqliteService() => new GitHubDbContext("github_data.sqlite");
    private readonly Dictionary<(int main, int sub), ICommand> _menuToCommand = new() 
    {
        { (0, 0), new GitHubApiService(() => CreateGitHubService()) },
        { (0, 1), new GetReposCommand(() => CreateCommandService()) },
        { (0, 2), new GetFollowersCommand(() => CreateCommandService()) },
        { (0, 3), new GetUserCommand(() => CreateCommandService()) },
        { (0, 4), new ShowDbCommand(() => CreateSqliteService()) },
        { (0, 5), new ClearDbCommand() },

        //{ (1, 0), new WebScrapingCommand() },
        //{ (1, 1), new ShowScrapedDataCommand() },
    };

    public void Run() 
    {
        while (true) 
        {
            int mainChoice = ShowMenu(_mainMenu);
            if (mainChoice == _mainMenu.Count - 1) return;
            Console.Clear();

            int subChoice = ShowMenu(_subMenus[mainChoice]);

            // var key = (mainChoice, subChoice);
            // if (_menuToCommand.TryGetValue(key, out var command))
            // {
            //     Clear();
            //     command.Execute();  // Полиморфизм!
            // }
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
