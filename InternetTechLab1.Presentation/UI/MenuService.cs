using System;
using static System.Console;
using System.Collections.Generic;
using InternetTechLab1.Commands;

namespace InternetTechLab1.UI;

public class MenuService
{
    private readonly List<string> _mainMenu;
    private readonly List<string>[] _subMenus;
    private readonly CommandFactory _commandFactory;

    public MenuService(CommandFactory commandFactory)
    {
        _commandFactory = commandFactory;

        _mainMenu = new List<string> 
        { 
            "Модуль A — Получение данных через API -> сохранение в реляционную БД", 
            "Модуль B — Web Scraping по URL -> сохранение в документоориентированную БД", 
            "Выход"
        };
        
        _subMenus = new[] 
        {
            new List<string> { "Получить данные о текущем пользователе", "Репозитории", "Подписчики пользователя", "Посмотреть базу данных", "Очистить базу данных", "Выход" },
            new List<string> { "Web Scraping по URL", "Посмотреть результаты Scraping", "Очистить базу данных", "Выход" }
        };
    }

    public async Task Run() 
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

                var command = _commandFactory.CreateCommand(mainChoice, subChoice);
                
                if (command != null)
                {
                    Clear();
                    await command.Execute();
                    WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
                    ReadKey();
                } 
                else 
                {
                    WriteLine("\nКоманда в разработке... Нажмите клавишу.");
                    ReadKey();
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
