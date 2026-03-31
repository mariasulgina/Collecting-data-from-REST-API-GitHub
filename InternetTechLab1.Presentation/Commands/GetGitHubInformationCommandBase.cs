using System.Threading.Tasks;
using InternetTechLab1.Core.Models;
using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

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

    public async Task Execute()
    {
        try 
        {
            Console.Write("Введите имя пользователя GitHub: ");
            string? username = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username))
            {
                await _logger.WriteLogToFile("[UI] Попытка вызова GitHub-команды с пустым именем пользователя");

                Console.WriteLine("Имя пользователя не может быть пустым");
            } else
            {
                await _logger.WriteLogToFile($"[Command] Запуск {this.GetType().Name} для пользователя: {username}");

                await ExecuteGitHubLogic(username);
            }
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFile($"[Fatal Error] Ошибка в {this.GetType().Name}: {ex.Message}");
            
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    protected abstract Task ExecuteGitHubLogic(string username);
}
