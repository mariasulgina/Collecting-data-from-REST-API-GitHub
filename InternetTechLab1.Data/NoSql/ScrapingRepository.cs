using InternetTechLab1.Models;
using System.Text.Json;
using System.IO;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Data;

public class ScrapingRepository : INonRelationalDatabaseService
{
    private readonly ILoggerService _logger;

    private readonly NoSqlSettings settings = new();
    private readonly string _path;

    public ScrapingRepository(ILoggerService logger)
    {
        _logger = logger;
        _path = settings.FilePath;
    }
    
    public async Task ClearDataBase()
    {
        await File.WriteAllTextAsync(_path, "[]");
        await _logger.WriteLogToFile($"[NoSQL] База данных (файл {_path}) успешно очищена");
    }

    public async Task SaveScrapeResults(IEnumerable<ScrapedItem> results)
    {
        List<ScrapedItem> allResults = new List<ScrapedItem>();

        if (File.Exists(_path))
        {
            string existingJson = await File.ReadAllTextAsync(_path);
            var oldItems = JsonSerializer.Deserialize<List<ScrapedItem>>(existingJson);
            
            if (oldItems != null)
            {
                allResults.AddRange(oldItems);
            }
        }

        allResults.AddRange(results);

        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonContent = JsonSerializer.Serialize(allResults, options);

        await File.WriteAllTextAsync(_path, jsonContent);
        await _logger.WriteLogToFile($"[NoSQL] Сохранено {results.Count()} новых записей в {_path}. Всего записей: {allResults.Count}");
    }

    public async Task<IEnumerable<ScrapedItem>> GetAllWebScrapResults()
    {
        IEnumerable<ScrapedItem> result = Enumerable.Empty<ScrapedItem>();

        if (File.Exists(_path))
        {
            string jsonContent = await File.ReadAllTextAsync(_path);
            result = JsonSerializer.Deserialize<List<ScrapedItem>>(jsonContent) ?? new List<ScrapedItem>();
            await _logger.WriteLogToFile($"[NoSQL] Успешно прочитано {result.Count()} записей из файла");
        }
        else
        {
            await _logger.WriteLogToFile($"[Error] Ошибка при чтении NoSQL");
        }

        return result;
    }
}
