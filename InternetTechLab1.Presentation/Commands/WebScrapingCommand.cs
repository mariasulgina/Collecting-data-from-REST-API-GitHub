using System.Threading.Tasks;
using InternetTechLab1.Models;
using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

public class WebScrapingCommand : ICommand
{
    protected readonly IScrapingService _scrapingService;
    protected readonly IVisualizerService _visualizer;
    protected readonly ILoggerService _logger;

    public WebScrapingCommand(IScrapingService scrapingService, IVisualizerService visualizer, ILoggerService logger)
    {
        _scrapingService = scrapingService;
        _visualizer = visualizer;
        _logger = logger;
    }

    public async Task Execute() 
    {
        Console.Write("Введите URL: ");
        string? urlname = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(urlname))
        {
            await _logger.WriteLogToFile("[UI] Попытка скрапинга с пустым URL");
            Console.WriteLine("URL не может быть пустым");
        } 
        else
        {
            await _logger.WriteLogToFile($"[UI] Запущена команда WebScraping для URL: {urlname}");

            IEnumerable<ScrapedItem>? results = await _scrapingService.GetFromURLWebScrapingInformation(urlname);

            if (results != null)
            {
                _visualizer.ShowScrapeResults(results);
                await _logger.WriteLogToFile($"[UI] Результаты для {urlname} успешно отображены пользователю");
            } 
            else
            {
                await _logger.WriteLogToFile($"[UI] Скрапинг {urlname} завершился без результатов");
                Console.WriteLine("Ничего не удалось найти по данному адресу");
            }
        }
    }
}
