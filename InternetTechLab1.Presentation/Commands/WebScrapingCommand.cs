using System.Threading.Tasks;
using System.Collections.Generic;
using InternetTechLab1.Models;
using InternetTechLab1.Services;
using InternetTechLab1.UI;

namespace InternetTechLab1.Commands;

public class WebScrapingCommand : ICommand
{
    protected readonly IScrapingService ScrapingService;
    protected readonly IVisualizerService Visualizer;

    public WebScrapingCommand(IScrapingService scrapingService, IVisualizerService visualizer)
    {
        ScrapingService = scrapingService;
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
            IEnumerable<ScrapedItem>? results = await ScrapingService.GetFromURLWebScrapingInformation(urlname);

            if (results != null)
            {
                Visualizer.ShowScrapeResults(results);
            } else
            {
                Console.WriteLine("Ничего не удалось найти по данному адресу");
            }
        }
    }
}
