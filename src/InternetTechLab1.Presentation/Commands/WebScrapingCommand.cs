using System.Threading.Tasks;
using InternetTechLab1.Core.Models;
using InternetTechLab1.UI;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Commands;

/// <summary>
/// Команда для выполнения веб-скрапинга по указанному URL-адресу.
/// Извлекает данные с веб-страницы, сохраняет их и передает на визуализацию.
/// </summary>
public class WebScrapingCommand : ICommand
{
    protected readonly IScrapingService _service;
    protected readonly IVisualizerService _visualizer;
    protected readonly ILoggerService _logger;

    public WebScrapingCommand(IScrapingService service, IVisualizerService visualizer, ILoggerService logger)
    {
        _service = service;
        _visualizer = visualizer;
        _logger = logger;
    }

    /// <summary>
    /// Основной цикл выполнения команды: запрос URL у пользователя, 
    /// обработка различных сетевых исключений и связанных с неверным адресом.
    /// </summary>
    public async Task ExecuteAsync() 
    {
        Console.Write("Введите URL: ");
        string? urlname = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(urlname))
        {
            await _logger.WriteLogToFileAsync("[UI] Попытка скрапинга с пустым URL");
            Console.WriteLine("URL не может быть пустым");
        } 
        else
        {
            try 
            {
                await _logger.WriteLogToFileAsync($"[UI] Запущена команда WebScraping для URL: {urlname}");

                IEnumerable<ScrapedItem>? results = await _service.GetFromURLWebScrapingInformationAsync(urlname);

                if (results != null)
                {
                    _visualizer.ShowScrapeResults(results);
                    await _logger.WriteLogToFileAsync($"[UI] Результаты для {urlname} успешно отображены пользователю");
                } 
                else
                {
                    await _logger.WriteLogToFileAsync($"[UI] Скрапинг {urlname} завершился без результатов");
                    Console.WriteLine("Ничего не удалось найти по данному адресу");
                }
            }
            catch (ArgumentException ex)
            {
                await _logger.WriteLogToFileAsync($"[UI Error] {ex.Message}");
                Console.WriteLine(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                await _logger.WriteLogToFileAsync($"[Network Error] {urlname}: {ex.Message}");
                Console.WriteLine($"Ошибка сети: Не удалось открыть сайт. Проверьте подключение.");
            }
            catch (Exception ex)
            {
                await _logger.WriteLogToFileAsync($"[Critical Error] {ex.Message}");
                Console.WriteLine($"Ошибка: {ex.Message}"); 
            }
        }
    }
}
