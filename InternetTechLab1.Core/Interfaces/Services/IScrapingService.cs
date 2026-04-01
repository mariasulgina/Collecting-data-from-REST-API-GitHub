using InternetTechLab1.Core.Models;

namespace InternetTechLab1.Core.Interfaces;

public interface IScrapingService : IService
{
    Task<IEnumerable<ScrapedItem>?> GetFromURLWebScrapingInformation(string url);
    Task<IEnumerable<ScrapedItem>> GetScrapedResultsAsync();
}
