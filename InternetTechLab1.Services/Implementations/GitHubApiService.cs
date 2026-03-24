using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

namespace InternetTechLab1.Services;

public class GitHubApiService : IGitHubApiService
{
    private const int _defaultTimeoutSeconds = 5;
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
            _httpClient.Timeout = TimeSpan.FromSeconds(_defaultTimeoutSeconds);
        }
    }

    public async Task<T?> GetFromApiGitHubInformationAsync<T>(string endpoint)
    {
        T? gitHubInformation = default;

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_defaultTimeoutSeconds)); 

            gitHubInformation = await _httpClient.GetFromJsonAsync<T>(endpoint, cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Ошибка: Время ожидания ответа от GitHub истекло");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка сети: Проверьте подключение к интернету ({ex.Message})");
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка API: {ex.Message}");
        }

        return gitHubInformation;
    }
}

