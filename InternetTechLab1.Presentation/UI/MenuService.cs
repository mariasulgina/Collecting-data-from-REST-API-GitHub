using static System.Console;
using InternetTechLab1.Commands;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.UI;

public class MenuService
{
    private readonly List<string> _mainMenu;
    private readonly List<string>[] _subMenus;
    private readonly CommandFactory _commandFactory;
    private readonly ILoggerService _logger;

    public MenuService(CommandFactory commandFactory, ILoggerService logger)
    {
        _commandFactory = commandFactory;
        _logger = logger;

        _mainMenu = new List<string> 
        { 
            "Модуль A — Получение данных через API -> сохранение в реляционную БД", 
            "Модуль B — Web Scraping по URL -> сохранение в документоориентированную БД", 
            "Выход"
        };
        
        _subMenus = new[] 
        {
            new List<string> { "Получить данные о текущем пользователе", "Репозитории", "Подписчики пользователя", "Посмотреть базу данных", "Очистить базу данных", "Найти пользователя в базе данных", "Выход" },
            new List<string> { "Web Scraping по URL", "Посмотреть результаты Scraping", "Очистить базу данных", "Выход" }
        };
    }

    public async Task RunAsync() 
    {
        await _logger.WriteLogToFileAsync("[System] Приложение запущено. Главное меню");
        bool isRunning = true;

        while (isRunning) 
        {
            Clear();
            int mainChoice = ShowMenu(_mainMenu);

            if (mainChoice == _mainMenu.Count - 1) 
            {
                await _logger.WriteLogToFileAsync("[System] Пользователь выбрал 'Выход'. Завершение работы");
                isRunning = false;
            }
            else
            {
                await _logger.WriteLogToFileAsync($"[UI] Переход в подменю: {_mainMenu[mainChoice]}");

                while (true) 
                {
                    Clear();
                    int subChoice = ShowMenu(_subMenus[mainChoice]);

                    if (subChoice == _subMenus[mainChoice].Count - 1) 
                    {
                        await _logger.WriteLogToFileAsync("[UI] Возврат в главное меню");
                        break;
                    }

                    var command = _commandFactory.CreateCommand(mainChoice, subChoice);
                    
                    if (command != null)
                    {
                        Clear();

                        await _logger.WriteLogToFileAsync($"[UI] Выбрана команда: {_subMenus[mainChoice][subChoice]}");

                        try
                        {
                            await command.ExecuteAsync();
                        }
                        catch (Exception ex)
                        {
                            await _logger.WriteLogToFileAsync($"[CRITICAL] Команда: {_subMenus[mainChoice][subChoice]} | Ошибка: {ex.Message} | StackTrace: {ex.StackTrace}");

                            ForegroundColor = ConsoleColor.Red;
                            WriteLine("\n[!] Ошибка выполнения команды");
                            ResetColor();

                            WriteLine($" Сообщение: {ex.Message}");
                        }

                        WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
                        ReadKey();
                    } 
                    else 
                    {
                        await _logger.WriteLogToFileAsync($"[Warning] Команда для выбора [{mainChoice}, {subChoice}] не найдена в Factory");
                        WriteLine("\nКоманда в разработке... Нажмите клавишу");
                        ReadKey();
                    }
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

            if (cki.Key == ConsoleKey.UpArrow)
            {
                currentPosition = Math.Max(currentPosition - 1, 0);
            }
            else if (cki.Key == ConsoleKey.DownArrow)
            {
                currentPosition = Math.Min(currentPosition + 1, stringItems.Count - 1);
            }
            else if (cki.Key == ConsoleKey.Enter)
            {
                Console.WriteLine($"\nВыбрано: {stringItems[currentPosition]}");
                return currentPosition;
            }
        } while (true);
    }
}
