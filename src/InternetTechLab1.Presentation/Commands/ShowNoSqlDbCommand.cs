using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

/// <summary>
/// Команда для отображения данных из NoSQL хранилища (результаты веб-скрапинга).
/// Читает данные из JSON-файла и выводит их через сервис визуализации.
/// </summary>
public class ShowNoSqlDbCommand : ICommand
{
    private readonly IScrapingService _service;
    private readonly IVisualizerService _visualizer;
    private readonly ILoggerService _logger;

    public ShowNoSqlDbCommand(IScrapingService service, IVisualizerService visualizer, ILoggerService logger)
    {
        _service = service;
        _visualizer = visualizer;
        _logger = logger;
    }

    /// <summary>
    /// Выполняет асинхронное чтение NoSQL базы и инициирует визуализацию.
    /// </summary>
    public async Task ExecuteAsync() 
    {
        try 
        {
            await _logger.WriteLogToFileAsync("[UI] Запрос на просмотр NoSQL базы данных (JSON)");
        
            var webScrapResults = await _service.GetScrapedResultsAsync();

            await _logger.WriteLogToFileAsync($"[UI] Из базы извлечено {webScrapResults?.Count() ?? 0} записей скрапинга");

            if (webScrapResults != null)
            {
                _visualizer.ShowNoSqlDb(webScrapResults);

                await _logger.WriteLogToFileAsync($"[UI] Результаты скрапинга успешно отображены пользователю");
            }
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFileAsync($"[Error] Ошибка при чтении NoSQL базы: {ex.Message}");
            Console.WriteLine($"Ошибка отображения данных: {ex.Message}");
        }
    }
}
