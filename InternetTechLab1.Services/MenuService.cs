using System;
using static System.Console;
using System.Collections.Generic;
using InternetTechLab1.Data.Rdbms;

namespace InternetTechLab1.Services;

public class MenuService
{
    private readonly List<string> _mainMenu;
    private readonly List<string>[] _subMenus;
    private readonly Dictionary<(int main, int sub), Func<ICommand>> _menuToCommandFactory;

    private readonly IGitHubApiService _gitHabApiService;
    private readonly IDatabaseService _databaseService;
    private readonly IGitHubScrapingService _gitHabScrapingService;

    public MenuService()
    {
        _databaseService = new GitHubRepository();
        _gitHabApiService = new GitHubApiService();
        _gitHabScrapingService = new GitHubScrapingService();

        _mainMenu = new List<string> 
        { 
            "Модуль A — Получение данных через API → сохранение в реляционную БД", 
            "Модуль B — Web Scraping по URL → сохранение в документоориентированную БД", 
            "Выход"
        };
        
        _subMenus = new[] 
        {
            new List<string> { "Получить данные о текущем пользователе", "Репозитории", "Подписчики пользователя", "Посмотреть базу данных", "Очистить базу данных", "Выход" },
            new List<string> { "Web Scraping по URL", "Посмотреть результаты Scraping", "Очистить базу данных", "Выход" }
        };

        _menuToCommandFactory = new Dictionary<(int main, int sub), Func<ICommand>>
        {
            { (0, 0), () => new GetCurrentUserCommand(_gitHabApiService, _databaseService) },
            { (0, 1), () => new GetReposCommand(_gitHabApiService, _databaseService) },
            { (0, 2), () => new GetFollowersCommand(_gitHabApiService, _databaseService) },
            { (0, 3), () => new ShowDbCommand(_databaseService, DataType.Api) },
            { (0, 4), () => new ClearDbCommand(_databaseService, DataType.Api) },

            { (1, 0), () => new WebScrapingCommand(_gitHabScrapingService, _databaseService) },
            { (1, 1), () => new ShowDbCommand(_databaseService, DataType.Scraping) },
            { (1, 2), () => new ClearDbCommand(_databaseService, DataType.Scraping) }
        };
    }

    public void Run() 
    {
        while (true) 
        {
            Clear();
            int mainChoice = ShowMenu(_mainMenu);
            if (mainChoice == _mainMenu.Count - 1) return;

            while (true) 
            {
                Clear();
                int subChoice = ShowMenu(_subMenus[mainChoice]);

                if (subChoice == _subMenus[mainChoice].Count - 1) break;

                var key = (mainChoice, subChoice);
                if (_menuToCommandFactory.TryGetValue(key, out var commandFactory))
                {
                    Clear();
                    commandFactory().Execute();

                    WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню");
                    ReadKey();
                } else 
                {
                    Console.WriteLine("Эта команда пока ничего не делает(");
                }
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
