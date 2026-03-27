using InternetTechLab1.Services;
using InternetTechLab1.Models;

namespace InternetTechLab1.Data.NoSql;

public class ScrapingRepository : INonRelationalDatabaseService
{
    private readonly NoSqlSettings _noSqlSettings = new();
    
    public async Task ClearDataBase()
    {
        
    }

    public void SaveScrapeResults(IEnumerable<ScrapedItem> results)
    {

    }
}
