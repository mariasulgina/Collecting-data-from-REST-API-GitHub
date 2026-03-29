using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;
using System.Linq;
using AngleSharp.Html.Parser;
using System.Net.Http;
using System.Net.Http.Json;
using AngleSharp.Html.Dom;
using System.Threading;
using InternetTechLab1.Models;
using AngleSharp.Dom;
using InternetTechLab1.Data;

namespace InternetTechLab1.Services;

public class ScrapingService : IScrapingService
{
    private readonly INonRelationalDatabaseService _database;
    private const int _defaultTimeoutSeconds = 50;
    private static HttpClient _httpClient = new HttpClient();

    public ScrapingService(INonRelationalDatabaseService database)
    {
        _database = database;

        if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "InternetTechLab1-App");
        }
        _httpClient.Timeout = TimeSpan.FromSeconds(_defaultTimeoutSeconds);
    }

    public async Task<IEnumerable<ScrapedItem>?> GetFromURLWebScrapingInformation(string url)
    {
        CancellationTokenSource cancellationToken = new CancellationTokenSource();

        HttpResponseMessage urlWebScrapingInformation = await _httpClient.GetAsync(url);
        cancellationToken.Token.ThrowIfCancellationRequested();

        string htmlContent = await urlWebScrapingInformation.Content.ReadAsStringAsync();
        cancellationToken.Token.ThrowIfCancellationRequested();

        HtmlParser parser = new HtmlParser();
        IHtmlDocument document = await parser.ParseDocumentAsync(htmlContent);

        IEnumerable<ScrapedItem> results = GetScrapeResults(document);
        await _database.SaveScrapeResults(results);

        return results;
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
