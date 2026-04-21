using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

/// <summary>
/// Команда для полной очистки всех баз данных (реляционной и NoSQL).
/// </summary>
public class ClearDbCommand : ICommand
{
    private readonly IService _service;
    private readonly ILoggerService _logger;

    public ClearDbCommand(IService service, ILoggerService logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Выполняет логику очистки данных. 
    /// </summary>
    public async Task ExecuteAsync() 
    {
        try 
        {
            await _logger.WriteLogToFileAsync("[UI] Запущена полная очистка базы данных");

            await _service.ClearAllDataAsync();

            await _logger.WriteLogToFileAsync("[Database] База данных успешно очищена");
            Console.WriteLine("База данных успешно очищена");
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFileAsync($"[Fatal Error] Не удалось очистить базу данных: {ex.Message}");
            Console.WriteLine($"Ошибка при очистке базы: {ex.Message}");
        }
    }
}
