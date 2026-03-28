using InternetTechLab1.Models;

namespace InternetTechLab1.Data;

public interface INonRelationalDatabaseService : IDatabaseService
{
    void SaveScrapeResults(IEnumerable<ScrapedItem> results);
}
