using System.Threading.Tasks;
using InternetTechLab1.Core.Models;
using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

/// <summary>
/// Базовый абстрактный класс для всех команд, взаимодействующих с GitHub API.
/// Реализует общий алгоритм: ввод логина, валидация, логирование и обработка исключений.
/// </summary>
public abstract class GetGitHubInformationCommandBase : ICommand
{
    protected readonly IGitHubService _service;
    protected readonly IVisualizerService _visualizer;
    protected readonly ILoggerService _logger;

    protected GetGitHubInformationCommandBase(IGitHubService service, IVisualizerService visualizer, ILoggerService logger)
    {
        _service = service;
        _visualizer = visualizer;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        try 
        {
            Console.Write("Введите имя пользователя GitHub: ");
            string? username = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username))
            {
                await _logger.WriteLogToFileAsync("[UI] Попытка вызова GitHub-команды с пустым именем пользователя");
                Console.WriteLine("Имя пользователя не может быть пустым");
            } else
            {
                await _logger.WriteLogToFileAsync($"[Command] Запуск {this.GetType().Name} для пользователя: {username}");
                await ExecuteGitHubLogicAsync(username);
            }
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFileAsync($"[Fatal Error] Ошибка в {this.GetType().Name}: {ex.Message}");
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    protected abstract Task ExecuteGitHubLogicAsync(string username);
}
