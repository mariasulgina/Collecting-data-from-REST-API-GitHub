using InternetTechLab1.Models;
using System.Text.Json;
using System.IO;

namespace InternetTechLab1.Data;

public class ScrapingRepository : INonRelationalDatabaseService
{
    private readonly NoSqlSettings _noSqlSettings = new();
    private readonly string _path = "scraped_data.json";
    
    public async Task ClearDataBase()
    {
        await File.WriteAllTextAsync(_path, "[]");
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
    }

    public async Task<IEnumerable<ScrapedItem>> GetAllWebScrapResults()
    {
        IEnumerable<ScrapedItem> result = Enumerable.Empty<ScrapedItem>();

        if (File.Exists(_path))
        {
            string jsonContent = await File.ReadAllTextAsync(_path);
            result = JsonSerializer.Deserialize<List<ScrapedItem>>(jsonContent) ?? new List<ScrapedItem>();
        }

        return result;
    }
}
