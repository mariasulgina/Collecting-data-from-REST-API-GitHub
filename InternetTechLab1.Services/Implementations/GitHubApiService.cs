using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

namespace InternetTechLab1.Services;

public class GitHubApiService : IGitHubApiService
{
    private static readonly HttpClient _httpClient = new HttpClient()
    {
        BaseAddress = new Uri("https://api.github.com/")
    };

    public GitHubApiService() 
    {
        if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "InternetTechLab1-App");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
        }
    }

    public async Task<T?> GetFromApiGitHubInformationAsync<T>(string endpoint) 
    {
        try
        {
            T? gitHubINformation = await _httpClient.GetFromJsonAsync<T>(endpoint);
            return gitHubINformation;
        } catch (Exception ex)
        {
            Console.WriteLine($"Ошибка API: {ex.Message}");
            return default;
        }
    }
}

