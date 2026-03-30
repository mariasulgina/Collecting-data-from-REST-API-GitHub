using System.Threading;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Services;

public class FileLogger : ILoggerService
{
    private readonly string _filePath = "log.txt";

    public FileLogger() { }

    public async Task WriteLogToFile(string message)
    {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] -- {message} -- {Environment.NewLine}";

        try 
        {
            await File.AppendAllTextAsync(_filePath, logEntry);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка логирования: {ex.Message}");
        }
    }
}
