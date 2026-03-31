using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

public class ShowDbCommand : ICommand
{
    private readonly IRelationalDatabaseService _relationalDatabaseService;
    private readonly IVisualizerService _visualizerService;
    private readonly ILoggerService _logger;

    public ShowDbCommand(IRelationalDatabaseService relationalDatabaseService, IVisualizerService visualizerService, ILoggerService logger)
    {
        _relationalDatabaseService = relationalDatabaseService;
        _visualizerService = visualizerService;
        _logger = logger;
    }

    public async Task Execute() 
    {
        try 
        {
            await _logger.WriteLogToFile("[UI] Запрос на просмотр реляционной базы (GitHub Data)");

            var users = await _relationalDatabaseService.GetAllUsers();
            _visualizerService.ShowDb(users);

            await _logger.WriteLogToFile($"[Database] Загружено пользователей: {users.Count}");

            var repos = await _relationalDatabaseService.GetAllRepos();
            _visualizerService.ShowDb(repos);

            await _logger.WriteLogToFile($"[Database] Загружено репозиториев: {repos.Count}");
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFile($"[Error] Ошибка при чтении реляционной БД: {ex.Message}");
            Console.WriteLine($"Ошибка при выводе базы: {ex.Message}");
        }
    }
}
