using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Parser;
using System.Net.Http;
using System.Net.Http.Json;

namespace InternetTechLab1.Services;

public class GitHubScrapingService : IGitHubScrapingService
{
    private const int _defaultTimeoutSeconds = 5;
    private static HttpClient _httpClient = new HttpClient();

    public GitHubScrapingService()
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "InternetTechLab1-App");
        _httpClient.Timeout = TimeSpan.FromSeconds(_defaultTimeoutSeconds);
    }

    public async Task<string?> GetFromURLWebScrapingInformation(string url)
    {
        HttpResponseMessage urlWebScrapingInformation = await _httpClient.GetAsync(url);
        var responce = await urlWebScrapingInformation.Content.ReadAsStringAsync();

        return responce;
    }
}
