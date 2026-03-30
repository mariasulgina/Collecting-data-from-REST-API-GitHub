using InternetTechLab1.Models;

namespace InternetTechLab1.Core.Interfaces;

public interface IScrapingService
{
    Task<IEnumerable<ScrapedItem>?> GetFromURLWebScrapingInformation(string url);
}
