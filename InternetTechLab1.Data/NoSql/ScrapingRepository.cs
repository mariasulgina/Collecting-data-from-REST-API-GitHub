using InternetTechLab1.Data.Models;
using InternetTechLab1.Core.Models;
using InternetTechLab1.Core.Interfaces;
using MongoDB.Driver;
using MongoDB.Bson;

namespace InternetTechLab1.Data;

public class ScrapingRepository : INonRelationalDatabaseService
{
    private readonly ILoggerService _logger;
    private readonly IMongoCollection<ScrapedItemEntity> _collection;

    public ScrapingRepository(ILoggerService logger)
    {
        _logger = logger;
        var settings = new NoSqlSettings();

        var client = new MongoClient(settings.Uri);
        var database = client.GetDatabase("scraped_data");
        _collection = database.GetCollection<ScrapedItemEntity>("scraped_items");
    }
    
    public async Task ClearDataBaseAsync()
    {
        await _collection.DeleteManyAsync(_ => true);
        await _logger.WriteLogToFileAsync($"[NoSQL] База данных успешно очищена");
    }

    public async Task SaveScrapeResultsAsync(IEnumerable<ScrapedItem> results)
    {
        int addedCount = 0;

        if (results != null)
        {
            var entities = results.Select(MapToEntity).ToList();
            await _collection.InsertManyAsync(entities);
            addedCount = entities.Count;
        }

        long count = await _collection.CountDocumentsAsync(_ => true);
        await _logger.WriteLogToFileAsync($"[NoSQL] Добавлено: {addedCount}");
    }

    public async Task<IEnumerable<ScrapedItem>> GetAllWebScrapResultsAsync()
    {
        var entities = await _collection.Find(_ => true).ToListAsync();
        var results = entities.Select(MapToDomain).ToList();

        await _logger.WriteLogToFileAsync($"[NoSQL] Прочитано из базы: {results.Count} записей");

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
