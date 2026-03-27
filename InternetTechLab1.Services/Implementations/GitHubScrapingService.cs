using System;
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

namespace InternetTechLab1.Services;

public class GitHubScrapingService : IGitHubScrapingService
{
    private const int _defaultTimeoutSeconds = 50;
    private static HttpClient _httpClient = new HttpClient();
    public string[] QueryTerms { get; } = { "title", "<h1>" };

    public GitHubScrapingService()
    {
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

        string responce = await urlWebScrapingInformation.Content.ReadAsStringAsync();
        cancellationToken.Token.ThrowIfCancellationRequested();

        HtmlParser parser = new HtmlParser();
        IHtmlDocument document = parser.ParseDocument(responce);
        Console.WriteLine($" {document.Title}");
        Console.WriteLine($" {document.QuerySelector("h1").TextContent}");
        Console.WriteLine($" {document.QuerySelector("h2").TextContent}");

        IEnumerable<ScrapedItem> results = GetScrapeResults(document);

        return results;
    }

    private IEnumerable<ScrapedItem> GetScrapeResults(IHtmlDocument htmlDocument)
    {
        List<IElement> allFound = new List<IElement>();

        foreach(var term in QueryTerms)
        {
            var matches = htmlDocument.All.Where(x => 
                x.ParentElement != null && 
                x.ParentElement.InnerHtml.Contains(term)
            );

            allFound.AddRange(matches);
        }

        return allFound.Distinct().Select(x => new ScrapedItem(
            Title : x.TextContent.Trim(),
            Content : x.InnerHtml
        )).ToList();
    }
}
