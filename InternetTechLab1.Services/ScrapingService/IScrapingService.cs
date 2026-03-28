using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public interface IScrapingService
{
    Task<IEnumerable<ScrapedItem>?> GetFromURLWebScrapingInformation(string url);
}
