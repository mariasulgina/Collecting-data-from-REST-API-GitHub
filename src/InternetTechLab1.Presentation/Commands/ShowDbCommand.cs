using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

/// <summary>
/// Команда для отображения всего содержимого локальной реляционной базы данных (GitHub пользователи и репозитории).
/// Используется для проверки сохраненных данных без выполнения API-запросов.
/// </summary>
public class ShowDbCommand : ICommand
{
    private readonly IGitHubService _service;
    private readonly IVisualizerService _visualizer;
    private readonly ILoggerService _logger;

    public ShowDbCommand(IGitHubService service, IVisualizerService visualizer, ILoggerService logger)
    {
        _service = service;
        _visualizer = visualizer;
        _logger = logger;
    }

    /// <summary>
    /// Выполняет последовательное чтение всех таблиц (пользователи, затем репозитории) и передает их в визуализатор.
    /// </summary>
    public async Task ExecuteAsync() 
    {
        try 
        {
            await _logger.WriteLogToFileAsync("[UI] Запрос на просмотр реляционной базы (GitHub Data)");

            var users = await _service.GetSavedUsersAsync();
            _visualizer.ShowDb(users);

            await _logger.WriteLogToFileAsync($"[Database] Загружено пользователей: {users.Count()}");

            var repos = await _service.GetSavedReposAsync();
            _visualizer.ShowDb(repos);

            await _logger.WriteLogToFileAsync($"[Database] Загружено репозиториев: {repos.Count()}");
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFileAsync($"[Error] Ошибка при чтении реляционной БД: {ex.Message}");
            Console.WriteLine($"Ошибка при выводе базы: {ex.Message}");
        }
    }
}
