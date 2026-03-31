using InternetTechLab1.Core.Models;

namespace InternetTechLab1.Core.Interfaces;

public interface INonRelationalDatabaseService : IDatabaseService
{
    Task SaveScrapeResults(IEnumerable<ScrapedItem> results);
    Task<IEnumerable<ScrapedItem>> GetAllWebScrapResults();
}
