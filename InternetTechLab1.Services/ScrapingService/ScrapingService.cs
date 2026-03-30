using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using InternetTechLab1.Models;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Services;

public class ScrapingService : IScrapingService
{
    private readonly INonRelationalDatabaseService _database;
    private readonly ILoggerService _logger;

    private static readonly int _defaultTimeoutSeconds = 50;
    private static readonly HttpClient _httpClient = new HttpClient();

    public ScrapingService(INonRelationalDatabaseService database, ILoggerService logger)
    {
        _database = database;
        _logger = logger;

        if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "InternetTechLab1-App");
        }
        _httpClient.Timeout = TimeSpan.FromSeconds(_defaultTimeoutSeconds);
    }

    public async Task<IEnumerable<ScrapedItem>?> GetFromURLWebScrapingInformation(string url)
    {
        try 
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();

            HttpResponseMessage urlWebScrapingInformation = await _httpClient.GetAsync(url);
            cancellationToken.Token.ThrowIfCancellationRequested();

            string htmlContent = await urlWebScrapingInformation.Content.ReadAsStringAsync();
            await _logger.WriteLogToFile($"[Service] HTML получен (размер: {htmlContent.Length} символов)");
            cancellationToken.Token.ThrowIfCancellationRequested();

            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = await parser.ParseDocumentAsync(htmlContent);

            IEnumerable<ScrapedItem> results = GetScrapeResults(document);
            await _logger.WriteLogToFile($"[Service] Парсинг завершен. Найдено элементов: {results.Count()}");

            await _database.SaveScrapeResults(results);
            await _logger.WriteLogToFile("[Service] Данные успешно сохранены в репозиторий");

            return results;
        }
        catch (Exception ex)
        {
            await _logger.WriteLogToFile($"[Error] Ошибка в ScrapingService: {ex.Message}");
            throw;
        }
    }

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
