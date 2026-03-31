using InternetTechLab1.Core.Models;

namespace InternetTechLab1.Core.Interfaces;

public interface IScrapingService
{
    Task<IEnumerable<ScrapedItem>?> GetFromURLWebScrapingInformation(string url);
}
