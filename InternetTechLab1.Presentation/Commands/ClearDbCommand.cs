using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

public class ClearDbCommand : ICommand
{
    private readonly IDatabaseService _databaseService;
    private readonly ILoggerService _logger;

    public ClearDbCommand(IDatabaseService databaseService, ILoggerService logger)
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    public async Task Execute() 
    {
        try 
        {
            await _logger.WriteLogToFile("[UI] Запущена полная очистка базы данных");

            await _databaseService.ClearDataBase();

            await _logger.WriteLogToFile("[Database] База данных успешно очищена");
            
            Console.WriteLine("База данных успешно очищена");
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFile($"[Fatal Error] Не удалось очистить базу данных: {ex.Message}");
            
            Console.WriteLine($"Ошибка при очистке базы: {ex.Message}");
        }
    }
}
