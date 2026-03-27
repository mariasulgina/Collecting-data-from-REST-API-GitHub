using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public interface INonRelationalDatabaseService : IDatabaseService
{
    void SaveScrapeResults(IEnumerable<ScrapedItem> results);
}
