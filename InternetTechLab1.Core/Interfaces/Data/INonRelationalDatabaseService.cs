using InternetTechLab1.Models;

namespace InternetTechLab1.Core.Interfaces;

public interface INonRelationalDatabaseService : IDatabaseService
{
    Task SaveScrapeResults(IEnumerable<ScrapedItem> results);
    Task<IEnumerable<ScrapedItem>> GetAllWebScrapResults();
}
