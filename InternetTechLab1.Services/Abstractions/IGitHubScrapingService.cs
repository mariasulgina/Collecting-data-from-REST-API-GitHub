using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public interface IGitHubScrapingService
{
    Task<IEnumerable<ScrapedItem>?> GetFromURLWebScrapingInformation(string url);
}
