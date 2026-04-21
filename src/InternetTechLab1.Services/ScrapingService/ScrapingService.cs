using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using InternetTechLab1.Core.Models;
using InternetTechLab1.Core.Interfaces;
using InternetTechLab1.Core.Settings;

namespace InternetTechLab1.Services;

/// <summary>
/// Сервис для выполнения веб-скрапинга (извлечения данных из HTML).
/// Позволяет получать заголовки, ссылки, мета-описания и текстовое содержимое страниц.
/// </summary>
public class ScrapingService : IScrapingService
{
    private readonly INonRelationalDatabaseService _database;
    private readonly ILoggerService _logger;
    private readonly ScrapingSettings _settings;

    /// <summary>
    /// Статический клиент HttpClient для выполнения HTTP-запросов.
    /// Настраивается один раз при первой инициализации сервиса.
    /// </summary>
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly object _lock = new object();
    private static bool _isInitialized = false;

    public ScrapingService(INonRelationalDatabaseService database, ILoggerService logger, ScrapingSettings settings)
    {
        _database = database;
        _logger = logger;
        _settings = settings;

        // Безопасная настройка заголовков и таймаута
        if (!_isInitialized)
        {
            lock (_lock)
            {
                if (!_isInitialized)
                {
                    _httpClient.DefaultRequestHeaders.Add("User-Agent", _settings.UserAgent);
                    _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
                    _isInitialized = true;
                }
            }
        }
    }

    /// <summary>
    /// Выполняет HTTP-запрос по указанному URL и извлекает структурированную информацию из HTML-документа.
    /// </summary>
    public async Task<IEnumerable<ScrapedItem>?> GetFromURLWebScrapingInformationAsync(string url)
    {
        try 
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentException("Введен некорректный формат URL. Не забудьте http:// или https://");
            }

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string htmlContent = await response.Content.ReadAsStringAsync();
            await _logger.WriteLogToFileAsync($"[Service] HTML получен (размер: {htmlContent.Length} символов)");

            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = await parser.ParseDocumentAsync(htmlContent);

            IEnumerable<ScrapedItem> results = GetScrapeResults(document);
            await _logger.WriteLogToFileAsync($"[Service] Парсинг завершен. Найдено элементов: {results.Count()}");

            await _database.SaveScrapeResultsAsync(results);
            await _logger.WriteLogToFileAsync("[Service] Данные успешно сохранены в репозиторий");

            return results;
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("404"))
        {
            throw new Exception("Сайт ответил ошибкой 404: Страница не найдена");
        }
        catch (HttpRequestException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
        {
            throw new Exception("Ошибка сети: Проверьте интернет-соединение или доступность сайта");
        }
        catch (TaskCanceledException)
        {
            throw new Exception($"Превышено время ожидания ({_settings.TimeoutSeconds} сек). Сайт слишком долго не отвечает");
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFileAsync($"[Error] Ошибка в ScrapingService: {ex.Message}");
            throw; 
        }
    }

    /// <summary>
    /// Полностью очищает все накопленные результаты скрапинга в NoSQL хранилище.
    /// </summary>
    public async Task ClearAllDataAsync() 
    {
        await _logger.WriteLogToFileAsync("[ScrapingService] Очистка NoSQL базы...");
        await _database.ClearDataBaseAsync();
    }

    /// <summary>
    /// Извлекает все ранее сохраненные результаты скрапинга из базы данных.
    /// </summary>
    public async Task<IEnumerable<ScrapedItem>> GetScrapedResultsAsync()
    {
        await _logger.WriteLogToFileAsync("[ScrapingService] Чтение результатов скрапинга из NoSQL...");
        return await _database.GetAllWebScrapResultsAsync();
    }

    /// <summary>
    /// Внутренний метод для разбора HTML-документа и поиска конкретных тегов (Title, H1, Links и т.д.).
    /// </summary>
    private IEnumerable<ScrapedItem> GetScrapeResults(IHtmlDocument document)
    {
        List<ScrapedItem> scrapedItems = new();
        string url = document.DocumentUri;

        scrapedItems.Add(new ScrapedItem {
            Url = url,
            DataType = "Title",
            Value = document.Title?.Trim() ?? "Без заголовка"
        });

        scrapedItems.Add(new ScrapedItem {
            Url = url,
            DataType = "H1",
            Value = document.QuerySelector("h1")?.TextContent?.Trim() ?? "H1 не найден"
        });

        scrapedItems.Add(new ScrapedItem {
            Url = url,
            DataType = "H2",
            Value = document.QuerySelector("h2")?.TextContent?.Trim() ?? "H2 не найден"
        });

        var links = document.QuerySelectorAll("a")
            .Take(5)
            .Select(a => new ScrapedItem {
                Url = url,
                DataType = "Link",
                Value = $"Текст: {a.TextContent?.Trim() ?? "без текста"}, Href: {a.GetAttribute("href") ?? "#"}"
            });
        scrapedItems.AddRange(links);

        var description = document.QuerySelector("meta[name='description']")?.GetAttribute("content");
        if (!string.IsNullOrEmpty(description))
        {
            scrapedItems.Add(new ScrapedItem {
                Url = url,
                DataType = "Meta-Description",
                Value = description
            });
        }

        var text = document.Body?.TextContent?.Trim() ?? "Нет текста";
        var textContent = text.Length > 500 ? text.Substring(0, 500) : text;
        scrapedItems.Add(new ScrapedItem {
            Url = url,
            DataType = "Text-Body",
            Value = textContent
        });

        var listItems = document.QuerySelectorAll("ul li")
            .Take(3)
            .Select(li => new ScrapedItem {
                Url = url,
                DataType = "ListItem",
                Value = li.TextContent?.Trim() ?? ""
            });
        scrapedItems.AddRange(listItems);

        return scrapedItems;
    }
}
