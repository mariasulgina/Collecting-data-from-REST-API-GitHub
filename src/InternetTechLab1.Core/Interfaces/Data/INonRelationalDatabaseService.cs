using InternetTechLab1.Core.Models;

namespace InternetTechLab1.Core.Interfaces;

public interface INonRelationalDatabaseService : IDatabaseService
{
    Task SaveScrapeResultsAsync(IEnumerable<ScrapedItem> results);
    Task<IEnumerable<ScrapedItem>> GetAllWebScrapResultsAsync();
}
