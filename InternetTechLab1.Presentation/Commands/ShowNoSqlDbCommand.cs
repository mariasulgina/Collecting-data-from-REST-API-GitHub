using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

public class ShowNoSqlDbCommand : ICommand
{
    private readonly INonRelationalDatabaseService _nonRelDb;
    private readonly IVisualizerService _visualizerService;
    private readonly ILoggerService _logger;

    public ShowNoSqlDbCommand(INonRelationalDatabaseService nonRelDb, IVisualizerService visualizerService, ILoggerService logger)
    {
        _nonRelDb = nonRelDb;
        _visualizerService = visualizerService;
        _logger = logger;
    }

    public async Task Execute() 
    {
        try 
        {
            await _logger.WriteLogToFile("[UI] Запрос на просмотр NoSQL базы данных (JSON)");
        
            var webScrapResults = await _nonRelDb.GetAllWebScrapResults();

            await _logger.WriteLogToFile($"[UI] Из базы извлечено {webScrapResults?.Count() ?? 0} записей скрапинга");

            _visualizerService.ShowNoSqlDb(webScrapResults);

            await _logger.WriteLogToFile($"[UI] Результаты скрапинга успешно отображены пользователю");
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFile($"[Error] Ошибка при чтении NoSQL базы: {ex.Message}");
            Console.WriteLine($"Ошибка отображения данных: {ex.Message}");
        }
    }
}
