using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;
using InternetTechLab1.Models;

namespace InternetTechLab1.Services;

public class WebScrapingCommand : ICommand
{
    protected readonly IGitHubScrapingService ScrapingService;
    protected readonly INonRelationalDatabaseService DbService;
    protected readonly IVisualizerService Visualizer;

    public WebScrapingCommand(IGitHubScrapingService gitHubScrapingService, INonRelationalDatabaseService nonRelationalDatabaseService, IVisualizerService visualizer)
    {
        ScrapingService = gitHubScrapingService;
        DbService = nonRelationalDatabaseService;
        Visualizer = visualizer;
    }

    public async Task Execute() 
    {
        Console.Write("Введите URL: ");
        string? urlname = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(urlname))
        {
            Console.WriteLine("URL не может быть пустым");
        } else
        {
            //IHtmlDocument? URLWebScrapingInformation = await ScrapingService.GetFromURLWebScrapingInformation(urlname);

            //if (URLWebScrapingInformation != null)
            //{
                //IEnumerable<ScrapedItem> results = GetScrapeResults(URLWebScrapingInformation);
                
                // if (results.Any())
                // {
                //     Visualizer.ShowScrapeResults(results);
                //     DbService.SaveScrapeResults(results);
                // }
            //}

            IEnumerable<ScrapedItem>? results = await ScrapingService.GetFromURLWebScrapingInformation(urlname);

            if (results != null)
            {
                //Visualizer.ShowScrapeResults(results);
                DbService.SaveScrapeResults(results);
            }
        }
    }
}
