using Microsoft.Extensions.Configuration;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Services;

public class FileLogger : ILoggerService
{
    private readonly string _filePath;
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public FileLogger(IConfiguration configuration) 
    {
        _filePath = configuration["LoggingSettings:LogFilePath"] ?? "log.txt";
    }

    public async Task WriteLogToFileAsync(string message)
    {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] -- {message} -- {Environment.NewLine}";

        await _semaphore.WaitAsync();
        try 
        {
            await File.AppendAllTextAsync(_filePath, logEntry);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка логирования: {ex.Message}");
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
