using InternetTechLab1.Data.Models;
using InternetTechLab1.Core.Models;
using System.Text.Json;
using System.IO;
using InternetTechLab1.Core.Interfaces;
using MongoDB.Driver;
using MongoDB.Bson;

namespace InternetTechLab1.Data;

public class ScrapingRepository : INonRelationalDatabaseService
{
    private readonly ILoggerService _logger;
    private readonly IMongoCollection<ScrapedItemEntity> _collection;
    private readonly NoSqlSettings settings = new();

    public ScrapingRepository(ILoggerService logger)
    {
        _logger = logger;

        var client = new MongoClient(settings.Uri);
        var database = client.GetDatabase("scraped_data");
        _collection = database.GetCollection<ScrapedItemEntity>("scraped_items");;
    }
    
    public async Task ClearDataBase()
    {
        await _collection.DeleteManyAsync(_ => true);
        await _logger.WriteLogToFile($"[NoSQL] База данных успешно очищена");
    }

    public async Task SaveScrapeResults(IEnumerable<ScrapedItem> results)
    {
        int addedCount = 0;

        if (results != null)
        {
            var entities = results.Select(MapToEntity).ToList();
            await _collection.InsertManyAsync(entities);
            addedCount = entities.Count;
        }

        long count = await _collection.CountDocumentsAsync(_ => true);
        await _logger.WriteLogToFile($"[NoSQL] Добавлено: {addedCount}");
    }

    public async Task<IEnumerable<ScrapedItem>> GetAllWebScrapResults()
    {
        var entities = await _collection.Find(_ => true).ToListAsync();
        var results = entities.Select(MapToDomain).ToList();

        await _logger.WriteLogToFile($"[NoSQL] Прочитано из базы: {results.Count} записей");

        return results;
    }

    private ScrapedItemEntity MapToEntity(ScrapedItem item) => new()
    {
        Url = item.Url,
        DataType = item.DataType,
        Value = item.Value
    };

    private ScrapedItem MapToDomain(ScrapedItemEntity entity) => new(
        entity.Url,
        entity.DataType,
        entity.Value
    );
}
