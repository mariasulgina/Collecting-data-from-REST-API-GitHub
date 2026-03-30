using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using InternetTechLab1.Core.Interfaces;

namespace InternetTechLab1.Services;

public class GitHubApiService : IGitHubApiService
{
    private readonly ILoggerService _logger;
    private static readonly int _defaultTimeoutSeconds = 50;
    private static readonly HttpClient _httpClient = new HttpClient()
    {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
        BaseAddress = new Uri("https://api.github.com/")
    };

    public GitHubApiService(ILoggerService logger) 
    {
        _logger = logger;

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
            await _logger.WriteLogToFile($"[GitHubAPI] Запрос: GET {endpoint}");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_defaultTimeoutSeconds)); 

            gitHubInformation = await _httpClient.GetFromJsonAsync<T>(endpoint, cts.Token);

            if (gitHubInformation != null)
            {
                await _logger.WriteLogToFile($"[GitHubAPI] Ответ от {endpoint} успешно получен и десериализован");
            }
            else
            {
                await _logger.WriteLogToFile($"[GitHubAPI] Предупреждение: API вернул пустой результат (null) для {endpoint}");
            }
        }
        catch (OperationCanceledException)
        {
            await _logger.WriteLogToFile($"[Error] Время ожидания ({_defaultTimeoutSeconds}с) истекло для {endpoint}");
            throw new Exception("Ошибка: Время ожидания ответа от GitHub истекло");
        }
        catch (HttpRequestException ex)
        {
            await _logger.WriteLogToFile($"[Error] Сетевая ошибка при запросе к ({endpoint}): {ex.Message}");
            throw new Exception($"Ошибка сети: Проверьте подключение к интернету ({ex.Message})");
        } 
        catch (Exception ex)
        {
            await _logger.WriteLogToFile($"[Error] Непредвиденная ошибка API ({endpoint}): {ex.Message}");
            throw new Exception($"Ошибка API: {ex.Message}");
        }

        return gitHubInformation;
    }
}
